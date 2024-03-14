aws configure set aws_access_key_id "ignore" --profile localstack
aws configure set aws_secret_access_key "ignore" --profile localstack
aws configure set region "eu-west-2" --profile localstack
aws configure set output "json" --profile localstack
aws configure set endpoint_url "http://localhost:4566" --profile localstack