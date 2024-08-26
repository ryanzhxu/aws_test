using System;
using Amazon.Lambda.Core;
using Amazon.Lambda.SQSEvents;
using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using System.Threading.Tasks;
using System.Collections.Generic;
using Amazon.Lambda.Serialization.SystemTextJson;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(DefaultLambdaJsonSerializer))]

namespace SqsTriggeredLambda_v2
{
    public class Function
    {
        private static readonly string DynamoDBTableName = "LambdaTriggeredDynamoDB";
        private static readonly IAmazonDynamoDB DynamoDbClient = new AmazonDynamoDBClient();

        /// <summary>
        /// This function is triggered by an SQS event, processes each message, and writes to DynamoDB.
        /// </summary>
        /// <param name="event">The SQS event containing the messages.</param>
        /// <param name="context">The ILambdaContext that provides methods for logging and describing the Lambda environment.</param>
        /// <returns>A Task representing the asynchronous operation.</returns>
        public async Task FunctionHandler(SQSEvent sqsEvent, ILambdaContext context)
        {
            foreach (var record in sqsEvent.Records)
            {
                var messageId = record.MessageId;
                var messageBody = record.Body;

                // Log the message details
                context.Logger.LogLine($"Processing message ID: {messageId}");
                context.Logger.LogLine($"Message body: {messageBody}");

                // Insert record into DynamoDB
                var putRequest = new PutItemRequest
                {
                    TableName = DynamoDBTableName,
                    Item = new Dictionary<string, AttributeValue>
                    {
                        { "MessageID", new AttributeValue { S = messageId } },
                        { "Value", new AttributeValue { S = messageBody } }
                    }
                };

                try
                {
                    await DynamoDbClient.PutItemAsync(putRequest);
                    context.Logger.LogLine($"Record added to DynamoDB with MessageID: {messageId}");
                }
                catch (Exception ex)
                {
                    context.Logger.LogLine($"Error adding record to DynamoDB: {ex.Message}");
                }
            }
        }
    }
}
