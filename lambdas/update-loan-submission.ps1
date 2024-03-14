cd .\LoanSubmission


function Execute-AWSCommand {
    param(
        [string]$command
    )
    try {
              Invoke-Expression "$command" 
        Write-Host "Successfully executed: $command"
    } catch {
        Write-Error "Error executing: $command"
        Write-Error $_.Exception.Message
        exit
    }
}

$zipFilePath = ".\bin\Release\net6.0\LoanSubmission.zip"
dotnet build
dotnet lambda package --configuration release --framework net6.0 --output-package $zipFilePath


$lambdaFunctionName = "LoanCheckerFunction"


Execute-AWSCommand "aws lambda update-function-code --function-name $lambdaFunctionName --zip-file ""fileb://$zipFilePath"" --profile localstack"


Execute-AWSCommand "aws lambda update-function-configuration --function-name $lambdaFunctionName --timeout 15 --profile localstack"



$lambdaFunctionName1 = "LoanSubmissionFunction"


Execute-AWSCommand "aws lambda update-function-code --function-name $lambdaFunctionName1 --zip-file ""fileb://$zipFilePath"" --profile localstack"


Execute-AWSCommand "aws lambda update-function-configuration --function-name $lambdaFunctionName1 --timeout 15 --profile localstack"