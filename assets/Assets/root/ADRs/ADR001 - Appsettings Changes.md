# ADR 001: Appsettings changes - 08/18/2025

//TODO: Expand on secret values needing \_ to traverse the json tree in nested values.

//TODO: Explain how to manage environmental variables for DevOps

## Context

Applications require environment specific variables, for example, the connection string for a database connection.
Different environments will point to different endpoints, and this extends into many different values, which is where the appsettings.json file comes into play.

Previously we would have a single appsettings.json file, and that file would be modified through automatic builds with variable replacement, or manual adjustment on the server.

This usage comes with a few disadvantages:

- Developers weren't sure what an environment's appsettings.json file looked like without access to the file.
- appsetting.json transformations through automated builds could become needlesly complex and would sometimes fail depending on what characters were used.
  - Containerization removes the ability to modify the appsetting file once the container is built.
- Load balanced servers needed manual adjustment if one of the servers needed to differ from another.
- Troubleshooting an environment such as Dev or Test would require the developer to modify the base appsettings.json file and undo changes afterwards.
- Saving secret values in appsetting.json can be a security issue
- Using strings to represent appsetting keys in code won't throw compile time errors if the key name was typo'd.

## Decision

In order to address these issues, we will now do the following:

- Use launchSettings.json to emulate launching a specific environment
- Use environment specific appsetting files
- Map the appsettings to a C# object
- Limit 'secrets' in source control to only expose Development and Testing environments.

## Getting Started

In order to work with the appsettings file, let's first define 'Appsetting' and 'Secrets'

### Definitions

- An _appsetting_ is a value that **CAN** be changed depending on the server without requiring a code change.
  - EX: Database connection string, report links, authority location, logging levels, etc.
  - Since appsettings are in a json format, we can define the parts of an Appsetting as follows:
    - `DefaultConnection: "localDb/MyDatabase"`
      - `DefaultConnection` is the **name** of the key
      - `"localDb/MyDatabase"` is the **value** of that key
- A _secret_ is an appsetting value that contains sensitive information.
  - EX: username/passwords
  - **If you think this value being shared could cause some form of harm, it's a secret.**
  - These we will be setting with environmental variables on the server or through launchSettings.json
  - :warning:`Secret **values** do not belong in the appsetting.json files, the secret **name** however can. We will discuss those later on.` :warning:

Since a secret is just an appsetting with sensitivity, we can now start to describe the appsetting files.

### Appsettings

- appsettings.json
  - This is the base file, consider it the skeleton in which all other appsetting.EVN.json files will use.
  - All appsetting values and secret names :warning: `only the names, not the values` :warning: should be included here.
    - Appsettings that aren't secrets can have values, but for secrets, just leave the value blank, ex:
      - Not a secret: `ApplicationName: "MyApp"`
      - Is a secret: `ApplicationAdminPassword: ""`
  - All values in this file should be represented in the C# AppSettings object.
  - All values should be set to values to be used in local development.
    - EX: Database connection string pointed to a local DB instance or whatever is being used for development.
  - Consider this file as insecure since it's checked into source control, so anything here **can** be exposed outside the development team.
- appsettings.ENV.json
  - These environment files represent the values that will be used in each environment. :question: `You can read more about environments in the README.md` :question:
  - Only values that differ from the base appsettings.json based on the environment go in the following files:
    - appsettings.Dev.json
    - appsettings.Test.json
    - appsettings.Stage.json
    - appsettings.Prod.json

### Secrets

Secrets are meant to be stored in the `ENVIRONMENT`, [which you can read more about here](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/environments?view=aspnetcore-9.0).
Basically the `ENVIRONMENT` variables are things we tell the server which is running your application about which will then be injected into the appsettings. This includes locally. To have a secret added, you're going to want to contact the development lead, but there may be some exceptions. In order to best illustrate this, please continue onto the [Example Workflow](#example-workflow) section.

To understand how these work, let's first understand how they work locally
in `launchSettings.json` :question: `You can read more about launchSettings.json in the README.md` :question:

In the profiles in `launchSettings.json` take the Api (Local) profile for example:

```json
"Api (Local)": {
    "commandName": "Project",
    "launchBrowser": true,
    "launchUrl": "appsettings",
    "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Local",
        "SECRET": "LocalSecret"
    },
    "dotnetRunMessages": true,
    "applicationUrl": "https://localhost:7236;http://localhost:5031"
},
```

Here we have a profile called 'Api (Local)' which can be executed through debugging (f5) in visual studio, any profiles we have configured here also will appear.
You'll notice the key `environmentVariables` has two values:

- `"ASPNETCORE_ENVIRONMENT": "Local",`
- `"SECRET": "LocalSecret",`

For `"ASPNETCORE_ENVIRONMENT": "Local"`, this is used to tell .net what appsettings.json to use. to use

```C#
var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
var builder = new ConfigurationBuilder()
.AddJsonFile("appsettings.json")
//Here we'll grab the right file.
.AddJsonFile($"appsettings.{environment}.json", optional: true)
.AddEnvironmentVariables()
.Build();
```

For `"SECRET": "LocalSecret"`, this will be 'injected' into the appsetting file assuming the Key name is the same as one found in the appsettings.json

I see you've got questions.

**Why not just store the value directly in the appsettings.json file?** \
_Because secrets are going to be injected via environmental variables, so when running locally, we're going to emulate as if were on the server._

**Ok sure, but you're still checking the secrets into source control** \
_Yeah...BUT we are agreeing to only check in values that aren't secrets that could influence anything above Test. This does also allow us to setup rules on check-in to strip this file if necessary without stripping the appsettings.json file._

### Example Workflow

Let's assume you wanted to add to your application an appsetting that allowed the application to read from an API, and required a key to do so. In this instance we might have the following:

- `ApiHelpUrl:"test.website.com/api/help"` - The URL where a user can get more information about the API.
- `ApiUrl:"test.website.com/api"` - The URL where the application will pull data from an API
- `ApiKey:"1234abc!!"` - The ApiKey we'll use to get access to the API URL.

To get these added, we'll follow these steps:

<details>
  <summary>1) Determine if these should be appsettings at all, and if so, are they secret?</summary>

- For our key `ApiHelpUrl`:
  - Is this a valid appsetting key? **YES!** Because this value almost certainly will need to change depending on the environment.
  - Now would also be a good time to get all the values we'll need from local to prod. In this scenario we'll use the following:
    - For Local, Dev, and Test, we'll use: `ApiHelpUrl:"test.website.com/help"`
    - For Stage, we'll use: `ApiHelpUrl:"uat.website.com/help"`
    - For Prod, we'll use: `ApiHelpUrl:"website.com/help"`
  - Now that we know the values we'll be using, is this a valid secret? **No!** Because we know at some point this will probably be exposed to the user, it stands to reason that a user should be able to visit this URL to get help, and therefore, does not need to be hidden.
- For our key `ApiUrl`:
  - Is this a valid appsetting key? **YES!** Because this value almost certainly will need to change depending on the environment.
  - Now would also be a good time to get all the values we'll need from local to prod. In this scenario we'll use the following:
    - For Local, Dev, and Test, we'll use: `ApiUrl:"test.website.com/api"`
    - For Stage, we'll use: `ApiUrl:"uat.website.com/api"`
    - For Prod, we'll use: `ApiUrl:"website.com/api"`
  - Is this a valid secret? **Probably not!** Because this may not be sensitive. If we intentionally hid this from the user by having the application responsible for calling the endpoint on the server side, then it's hidden anyway, but there's a good chance that because there's a help page for this URL, that there's also a page explaining how to call it outside our app. Again, not for sure, but this likely isn't 'secret'. For this example, we'll assume it's not a secret.
- For our key `ApiKey`:
  - Is this a valid appsetting key? **YES!** Because this value almost certainly will need to change depending on the environment.
  - Now would also be a good time to get all the values we'll need from local to prod. In this scenario we'll use the following:
    - For Local, Dev, and Test, we'll use: `ApiKey:"1234abc!!`
    - For Stage, we'll use: `ApiKey:"1234abc!!`
    - For Prod, we'll use: `ApiKey:"I3jo2)382&$#98392kd%%*#&@`
  - Is this a valid secret? **YES!** This one is obvious, but this being a key to the API, is really just a form of password. So we know this is a secret, but there's something to note. The Local/Dev/Test key is the same value as the Stage, this complicates things.
    - Ideally we'd ask for a password that is different for Stage, because with this being the case, our 'Stage' data is the same as something like 'Local', which means it's comprimised the moment we check-in code.
    - Because we will ensure no secrets for Stage to Prod are in source control, this would effectively circumvent that rule, if this is the case, we need to discuss what specifically to do.
    - For the rest of this example, we'll assume that we were able to get the Stage value changed to: `ApiKey:"23923jk#@(j2`

</details>

<details>
 <summary>2) Modify C# AppSettings object</summary>  

Modify the AppSettings object to include these new keys so our application can read them in.

```c#
  public class AppSettings
  {
      ...
      public string ApiHelpUrl { get; set; }
      public string ApiUrl { get; set; }
      public string ApiKey { get; set; }
      ...
  }
```

</details>

<details>
 <summary>3) Add the keys with values to the appsettings.json files</summary>

```json
    //appsettings.json
    //Since these values are used for Local development here, we'll add them here.
    {
        ...
        "ApiHelpUrl":"test.website.com/help"
        "ApiUrl":"test.website.com/api"
        "ApiKey":"" //This value isn't added here since we know it's a secret.
        ...
    }
```

```json
    //appsettings.Dev.json
    //Since these values don't change between dev and local, we don't need to even include them.
    //Only include values that overwrite the base appsettings.json
    {
        ...
        ...
    }
```

```json
    //appsettings.Test.json
    //Since these values don't change between test and local, we don't need to even include them.
    //Only include values that overwrite the base appsettings.json
    {
        ...
        ...
    }
```

```json
    //appsettings.Stage.json
    //Since these values do change between test and local, we need to define them to overwrite
    {
        ...
        "ApiHelpUrl":"uat.website.com/help"
        "ApiUrl":"uat.website.com/api"
        ...
    }
```

```json
    //appsettings.Prod.json
    //Since these values do change between test and local, we need to define them to overwrite
    {
        ...
        "ApiHelpUrl":"website.com/help"
        "ApiUrl":"website.com/api"
        ...
    }
```

</details>

<details>
 <summary>4) Modify launchSettings.json</summary>  

launchSettings.json is where you're able to define what environment you'll run locally.
Unless you're running the application against the Stage/Prod environment locally, we don't
need to define those in our launchSettings, only the project team needs to know those secrets in order to configure the server environment to read from them.

```json
"Api (Local)": {
  ...
  "environmentVariables": {
      "ASPNETCORE_ENVIRONMENT": "Local",
      "ApiKey": "1234abc!!" //Added here, not appsetting.json
  },
  ...
},
```

```json
"Api (Dev)": {
  ...
  "environmentVariables": {
      "ASPNETCORE_ENVIRONMENT": "Local",
      "ApiKey": "1234abc!!" //Added here, not appsetting.json
  },
  ...
},
```

```json
"Api (Test)": {
  ...
  "environmentVariables": {
      "ASPNETCORE_ENVIRONMENT": "Local",
      "ApiKey": "1234abc!!" //Added here, not appsetting.json
  },
  ...
},
```

</details>

## Consequences

What becomes easier or more difficult to do because of this change?

### :heavy_check_mark:Pros

- Defines what secrets go into source control
- Allows developers to know exactly what is being used in appsettings.json for all environments
- Allows developers to know immediately what values actually change between environments
- Minimizes communication of appsetting changes, since non-secret values are clearly defined for every environment
- Accounts for cloud development and containerization without requiring appsetting transformation

### :heavy_multiplication_x:Cons

- Requires developers to know secrets are to be stored as an injected variable in launchSettings.json which isn't intuitive
- Requires developers to think about environment appsettings in an 'inherited'
- Requires team to address all appsetting values for all environments from the onset

## Additional Information

.Net Core applications don't reset on appsetting.json changes, it requires the app pool to be recycled. However, if you're needing to reset Dev/Test easily, just modify the web.config file, which will refresh the apppool and pick up the changes in the appsetting.json files
