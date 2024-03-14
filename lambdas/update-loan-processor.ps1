cd .\LoanProcessor

$zipFilePath = "bin\Release\net6.0\LoanProcessor.zip"
dotnet build
dotnet lambda package --configuration release --framework net6.0 --output-package $zipFilePath


$lambdaFunctionName = "LoanProcessorFunction"


aws lambda update-function-code --function-name $lambdaFunctionName --zip-file "fileb://.\$zipFilePath" --profile localstack


aws lambda update-function-configuration --function-name $lambdaFunctionName --timeout 15 --profile localstack