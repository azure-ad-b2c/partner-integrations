# IDology Azure AD B2C Integration

IDology is an ID verification service that can be used to verify
identification provided by users.

## Solution Components

The IDology integration is comprised of the following components:

  - Azure AD B2C – The authorization server, responsible for verifying
    the user’s credentials. It is also known as the identity provider

  - IDology – The IDology service takes inputs provided by the user and
    verifies the user’s identity

  - Custom Rest API – This provided API implements the integration
    between Azure AD and the IDology service.

## Create an IDology account

When you are ready to get an IDology account, sign up at xxxx

## Deploy the API

Deploy the provided API code to an Azure service. The code can be
published from Visual Studio, following these
[instructions](https://docs.microsoft.com/visualstudio/deployment/quickstart-deploy-to-azure?view=vs-2019).

Note the URL of the deployed service. This will be needed to configure
Azure AD with the required settings.

### Configure the API

Application settings can be [configured in the App service in
Azure](https://docs.microsoft.com/en-us/azure/app-service/configure-common#configure-app-settings).
This allows for settings to be securely configured without checking them
into a repository. The Rest API needs the following settings provided:

| Application Setting Name    | Source                        | Notes                                        |
| --------------------------- | ----------------------------- | -------------------------------------------- |
| IdologySettings:ApiUsername | IDology account configuration |                                              |
| IdologySettings:ApiPassword | IDology account configuration |                                              |
| WebApiSettings:ApiUsername  | Define a username for the API | This will be used in the ExtId configuration |
| WebApiSettings:ApiPassword  | Define a password for the API | This will be used in the ExtId configuration |

## Azure AD B2C Configuration

The provided sample policy is based on the

### Create API Policy Keys

Following [this
documentation](https://docs.microsoft.com/en-us/azure/active-directory-b2c/secure-rest-api#add-rest-api-username-and-password-policy-keys),
create two policy keys – one for the API Username, and one for the API
password which you defined above.

The sample policy uses these key names:

  - B2C\_1A\_RestApiUsername

  - B2C\_1A\_RestApiPassword

### Update API URL

In the section “Idology-ExpectId-API”, update the ServiceUrl metadata
item with the location of the API deployed above.

### Configure the B2C Policy

Follow [this
documentation](https://docs.microsoft.com/en-us/azure/active-directory-b2c/custom-policy-get-started?tabs=applications#custom-policy-starter-pack)
to configure the policy for the B2C tenant.

# Notes

This sample policy is based on [LocalAccounts starter
pack](https://github.com/Azure-Samples/active-directory-b2c-custom-policy-starterpack/tree/master/LocalAccounts).
