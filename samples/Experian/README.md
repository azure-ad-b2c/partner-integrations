# Experian B2C Integration

[Experian](https://www.experian.com/decision-analytics/account-opening-fraud/microsoft-integration) is an ID verification service that can be used to verify
identification provided by users.

This integration asks the user for several identifying pieces of
information and uses Experian to determine whether the user should be
allowed to continue or not. The following attributes are used in making
a pass/fail decision:

  - Email

  - IP Address

  - Given Name

  - Middle Name

  - Surname

  - Street Address

  - City

  - State/Province

  - Postal Code

  - Country/Region

  - Phone Number

## Solution Components

The Experian integration is comprised of the following components:

  - Azure AD B2C – The authorization server, responsible for verifying
    the user’s credentials. It is also known as the identity provider

  - Experian – The Experian service takes inputs provided by the user
    and verifies the user’s identity

  - Custom Rest API – This provided API implements the integration
    between Azure AD and the Experian service.

## Create an Experian account

When you are ready to get an Experian account, sign up at xxxx

## Deploy the API

Deploy the provided API code to an Azure service. The code can be
published from Visual Studio, following these
[instructions](https://docs.microsoft.com/visualstudio/deployment/quickstart-deploy-to-azure?view=vs-2019).

Note the URL of the deployed service. This will be needed to configure
Azure AD with the required settings.

### Deploy the client certificate

The Experian API call is protected by a client certificate. This client
certificate will be provided by Experian. This certificate must be
uploaded to the Azure App service using these
[instructions](https://docs.microsoft.com/en-us/azure/app-service/environment/certificates#private-client-certificate).
Two key steps in this process are required:

  - Upload the certificate

  - Set the WEBSITE\_LOAD\_ROOT\_CERTIFICATES key with the thumbprint of
    the certificate.

### Configure the Custom REST API

Application settings can be [configured in the App service in
Azure](https://docs.microsoft.com/en-us/azure/app-service/configure-common#configure-app-settings).
This allows for settings to be securely configured without checking them
into a repository. The REST API needs the following settings provided:

| Application Setting Name        | Source                         | Notes                                        |
| ------------------------------- | ------------------------------ | -------------------------------------------- |
| CrossCoreConfig:TenantId        | Experian account configuration |                                              |
| CrossCoreConfig:OrgCode         | Experian account configuration |                                              |
| CrossCore:ApiEndpoint           | Experian account configuration |                                              |
| CrossCore:ClientReference       | Experian account configuration |                                              |
| CrossCore:ModelCode             | Experian account configuration |                                              |
| CrossCore:OrgCode               | Experian account configuration |                                              |
| CrossCore:SignatureKey          | Experian account configuration |                                              |
| CrossCore:TenantId              | Experian account configuration |                                              |
| CrossCore:CertificateThumbprint | Experian certificate           |                                              |
| BasicAuth:ApiUsername           | Define a username for the API  | This will be used in the ExtId configuration |
| BasicAuth:ApiPassword           | Define a password for the API  | This will be used in the ExtId configuration |

## Azure AD B2C Configuration

### Create API Policy Keys

Following [this
documentation](https://docs.microsoft.com/en-us/azure/active-directory-b2c/secure-rest-api#add-rest-api-username-and-password-policy-keys),
create two policy keys – one for the API Username, and one for the API
password which you defined above.

The sample policy uses these key names:

  - B2C\_1A\_RestApiUsername

  - B2C\_1A\_RestApiPassword

### Update API URL

In the provided TrustFrameworkExtensions policy, find the Technical
Profile named “REST-Experian”, and update the ServiceUrl metadata item
with the location of the API deployed above.

### Configure the B2C Policy

Follow [this
documentation](https://docs.microsoft.com/en-us/azure/active-directory-b2c/custom-policy-get-started?tabs=applications#custom-policy-starter-pack)
to configure the policy for the B2C tenant. The provided policies will
need to be updated to relate to your specific tenant.

# Notes

This sample policy is based on [LocalAccounts starter
pack](https://github.com/Azure-Samples/active-directory-b2c-custom-policy-starterpack/tree/master/LocalAccounts).
