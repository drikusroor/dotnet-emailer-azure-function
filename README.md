# Dotnet Emailer Azure Function

This is a very simple Azure Function example that allows you to send emails using an SMTP connection. It written using `.NET 6` and uses the `Mailkit` nuget package.

## Getting started

### Prequisites

- Make sure the [.NET 6 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) has been installed
- Make sure [Azure Functions Core Tools](https://github.com/Azure/azure-functions-core-tools) has been installed (`npm i -g azure-functions-core-tools@4 --unsafe-perm true`)

### Configure environment variables

Configure the following `SMTP_` variables in your `local.settings.json` file (with your own SMTP credentials):

```js
{
  // ...
  "Values": {
    // ...
    "SMTP_SERVER": "your_smtp_server",
    "SMTP_PORT": "587",
    "SMTP_SSL": "true",
    "SMTP_USERNAME": "your_username",
    "SMTP_PASSWORD": "your_password"
  }
}
```

### Run Azure Function using Azure Functions Core Tools

Then, run:

```sh
func start
```

It should give you the Azure Function url that you can call and probably looks something like this:

```sh
send_email: [POST] http://localhost:7071/api/send_email
```

### Make the HTTP Request

Using your HTTP client of choice, post the following json data to the endpoint:

*Note: you should be able to use HTML(!)*

```js
// POST: http://localhost:7071/api/send_email
{
    "To": "info@example.com",
    "Subject": "Hello from Azure Functions!",
    "Body": "<h1>An awesome email has arrived</h1> \n\n <p>This is a test email sent from an Azure Function using SMTP.</p>",
    "Important": true
}
```
