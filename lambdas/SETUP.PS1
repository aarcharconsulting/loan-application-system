$ErrorActionPreference = "Stop"

$profile = "localstack"

function Execute-AWSCommand {
    param(
        [string]$command
    )
    try {
        Invoke-Expression "$command" | Out-Null
        Write-Host "Successfully executed: $command"
    } catch {
        Write-Error "Error executing: $command"
        Write-Error $_.Exception.Message
        exit
    }
}

function Check-SQSQueue {
    param(
        [string]$queueName
    )
    $queues = aws sqs list-queues --profile $profile --query "QueueUrls" | ConvertFrom-Json
    return ($queues -match $queueName)
}

function Start-LocalStack {
    try {
        docker-compose up -d
        Write-Host "LocalStack started"
    } catch {
        Write-Error "Failed to start LocalStack"
        exit
    }
}

function Create-SQSQueue {
    param(
        [string]$queueName
    )
    if (-not (Check-SQSQueue -queueName $queueName)) {
        Execute-AWSCommand "aws sqs create-queue --queue-name $queueName --profile $profile"
    } else {
        Write-Host "Queue $queueName already exists."
    }
}

function Deploy-LambdaFunction {
    param(
        [string]$functionName,
        [string]$runtime,
        [string]$role,
        [string]$handler,
        [string]$codeUri,
        [int]$timeout
    )

    $lambdaExists = aws lambda list-functions --profile $profile --query "Functions[?FunctionName=='$functionName'].FunctionName" | ConvertFrom-Json
    if ($lambdaExists.Count -eq 0) {
        Execute-AWSCommand "aws lambda create-function --function-name $functionName --output table --timeout $timeout --runtime $runtime --role $role --handler $handler --zip-file fileb://$codeUri --profile $profile" 
    } else {
        Write-Host "Lambda function $functionName already exists."
    }
  
}

function Deploy-LoanProcessor {
    $currentDirectoryFullPath = (Get-Location).Path
    cd ./LoanProcessor

    $zipFile ="./bin/Release/net6.0/LoanProcessor.zip"
    $functionName = "LoanProcessorFunction"
    Build-And-Package-DotNet -packageUri $zipFile

    Deploy-LambdaFunction -functionName $functionName `
                          -runtime "dotnet6" `
                          -role "arn:aws:iam::123456789012:role/execution_role" `
                          -handler "LoanProcessor::LoanProcessor.LoanProcessorFunction::FunctionHandler" `
                          -codeUri $zipFile `
                          -timeout 15

  
    aws lambda add-permission --function-name "LoanProcessorFunction" --statement-id "loanprocessortwominrule"  --action "lambda:InvokeFunction"  --principal "sqs.amazonaws.com"  --source-arn "arn:aws:sqs:eu-west-2:000000000000:request-queue"   --region "eu-west-2" --profile $profile

    aws lambda create-event-source-mapping --function-name "arn:aws:lambda:eu-west-2:000000000000:function:LoanProcessorFunction"  --batch-size 10  --event-source-arn "arn:aws:sqs:eu-west-2:000000000000:request-queue" --region "eu-west-2" --profile $profile

    cd $currentDirectoryFullPath

    Write-Host "Deployment of loanprocessor completed successfully."
}

function Deploy-LoanSubmission {
    
    $currentDirectoryFullPath = (Get-Location).Path
    cd ./LoanSubmission
    
    $zipFilePath = ".\bin\Release\net6.0\LoanSubmission.zip"

    Build-And-Package-DotNet -packageUri $zipFilePath

    Deploy-LambdaFunction -functionName "LoanSubmissionFunction" -runtime "dotnet6" -role "arn:aws:iam::000000000000:role/FakeRole" -handler "LoanSubmission::LoanSubmission.LoanSubmissionFunction::LoanSubmissionFunctionHandler" -codeUri $zipFilePath -timeout 15

    Deploy-LambdaFunction -functionName "LoanCheckerFunction" -runtime "dotnet6" -role "arn:aws:iam::000000000000:role/FakeRole" -handler "LoanSubmission::LoanSubmission.LoanSubmissionFunction::LoanCheckerFunctionHandler" -codeUri $zipFilePath -timeout 15

    Create-And-Configure-API

    cd $currentDirectoryFullPath
    Write-Host "Deployment of LoanSubmission started."
}
 
function Build-And-Package-DotNet {
     param(
        [string]$packageUri
    )
    try {

        dotnet clean --configuration release 
        dotnet build --configuration release 
        dotnet lambda package --configuration release --framework net6.0 --output-package $packageUri

        Write-Host "Build and package successful"
    } catch {
        Write-Error "Failed to build and package .NET application"
        exit
    }
}

function Create-And-Configure-API {
    $localstackBaseURL = "http://localhost:4566/restapis"
    $apiName = "LoanSubmissionAPI"

    $apiExists = aws apigateway get-rest-apis --profile $profile --query "items[?name=='$apiName'].id" | ConvertFrom-Json
    if ($apiExists.Count -eq 0) {
        $apiId = aws apigateway create-rest-api --name $apiName --profile $profile --query 'id' | ConvertFrom-Json
        Write-Host "API '$apiName' created with ID: $apiId"
    } else {
        $apiId = $apiExists[0]
        Write-Host "API '$apiName' already exists with ID: $apiId"
    }

    $parentId = aws apigateway get-resources --rest-api-id $apiId --profile $profile --query "items[?path=='/'].id" | ConvertFrom-Json
    $resourceLoanSubmissionId = Create-API-Resource -apiId $apiId -parentId $parentId -pathPart 'requestloan'
    $resourceLoanCheckerId = Create-API-Resource -apiId $apiId -parentId $parentId -pathPart 'loanresults'

    Configure-API-Method -apiId $apiId -resourceId $resourceLoanSubmissionId -httpMethod 'POST'  -functionName "LoanSubmissionFunction"
    Configure-API-Method -apiId $apiId -resourceId $resourceLoanCheckerId -httpMethod 'GET'  -functionName "LoanCheckerFunction"

    $deploymentId = aws apigateway create-deployment --rest-api-id $apiId --stage-name test --profile $profile --query 'id' | ConvertFrom-Json
    
    $loanSubmissionUrl = "$localstackBaseURL/$apiId/test/_user_request_/requestloan"
    $loanCheckerUrl = "$localstackBaseURL/$apiId/test/_user_request_/loanresults"

    Write-Host "Use the following URLs to invoke your API:"
    Write-Host "Loan Submission POST URL: $loanSubmissionUrl"
    Write-Host "Loan Checker GET URL: $loanCheckerUrl"
}

function Create-API-Resource {
    param(
        [string]$apiId,
        [string]$parentId,
        [string]$pathPart
    )
     $resourceId = aws apigateway create-resource --rest-api-id $apiId --parent-id $parentId --path-part $pathPart --profile $profile --query 'id' | ConvertFrom-Json
     return $resourceId 
}

function Configure-API-Method {
    param(
        [string]$apiId,
        [string]$resourceId,
        [string]$httpMethod,
        [string]$functionName
    )
    $lambdaArn = "arn:aws:lambda:eu-west-2:000000000000:function:$functionName"
    $uri = "arn:aws:apigateway:us-east-1:lambda:path/2015-03-31/functions/$lambdaArn/invocations"

    Execute-AWSCommand "aws apigateway put-method --rest-api-id $apiId --resource-id $resourceId --http-method $httpMethod --authorization-type NONE --profile $profile"
    Execute-AWSCommand "aws apigateway put-integration --rest-api-id $apiId --resource-id $resourceId --http-method $httpMethod --type AWS_PROXY --integration-http-method $httpMethod --uri $uri --profile $profile"
}

Start-LocalStack
Create-SQSQueue -queueName "request-queue"
Create-SQSQueue -queueName "response-queue"
Write-Host "Deployment of queue and localstack completed successfully."

Deploy-LoanProcessor
Deploy-LoanSubmission
