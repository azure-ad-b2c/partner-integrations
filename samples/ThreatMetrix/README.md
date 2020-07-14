ThreatMetrix B2C Integration
============================

ThreatMetrix is a profiling and identity validation service that can be
used to verify identification provided by users and provide
comprehensive risk assessments based on the user's device.

This integration performs a profiling step during the sign-up flow and
asks the user for a few pieces of information. As the sign-up step
completes, a call is made to ThreatMetrix to determine the risk
assessment based on the user's information and device profiling. The
following attributes are used in ThreatMetrix's risk analysis:

-   Email

-   Phone Number

-   Profiling information collected from the user's machine

Solution Components
-------------------

The ThreatMetrix integration is comprised of the following components:

-   Azure AD B2C -- The authorization server, responsible for verifying
    the user's credentials. It is also known as the identity provider

-   ThreatMetrix -- The ThreatMetrix service takes inputs provided by
    the user and combines it with profiling information gathered from
    the user's machine to verify the security of the user interaction.

-   Custom REST API -- This provided API implements the integration
    between Azure AD and the ThreatMetrix service.

Create a ThreatMetrix account
-----------------------------

When you are ready to get a ThreatMetrix account, sign up at xxxx

### Create a ThreatMetrix policy

Using the documentation available here (insert LexisNexis link), create
a Threat Metrix policy that meets your requirements. Note the name of
the policy for use later.

Deploy the API
--------------

Deploy the provided API code to an Azure service. The code can be
published from Visual Studio, following these
[instructions](https://docs.microsoft.com/visualstudio/deployment/quickstart-deploy-to-azure?view=vs-2019).

Note the URL of the deployed service. This will be needed to configure
Azure AD with the required settings.

### Configure the Custom REST API

Application settings can be [configured in the App service in
Azure](https://docs.microsoft.com/en-us/azure/app-service/configure-common#configure-app-settings).
This allows for settings to be securely configured without checking them
into a repository. The REST API needs the following settings provided:

  Application Setting Name  | Source                                  | Notes
  --------------------------| ----------------------------------------| --------------------------------------------
  ThreatMetrix: Url         | ThreatMetrix account configuration      | 
  ThreatMetrix:OrgId        | ThreatMetrix account configuration      | 
  ThreatMetrix:ApiKey       | ThreatMetrix account configuration      | 
  ThreatMetrix:Policy       | Name of policy created in ThreatMetrix  | 
  BasicAuth:ApiUsername     | Define a username for the API           | This will be used in the B2C configuration
  BasicAuth:ApiPassword     | Define a password for the API           | This will be used in the B2C configuration

Deploy the UI
-------------

This solution makes use of custom UI templates that are loaded by Azure
AD B2C. These UI templates perform the profiling that is sent directly
to the ThreatMetrix service.

[These
instructions](https://docs.microsoft.com/en-us/azure/active-directory-b2c/custom-policy-ui-customization#custom-page-content-walkthrough)
can be referenced to deploy the included UI files to a blob storage
account, including setting up a blob storage account, configuring CORS,
and enabling public access.

The UI is based on the ocean blue page template. All links within the UI
should be updated to refer to the deployed location. A find and replace
can be done throughout the UI folder to change
"https://yourblobstorage/blobcontainer" to the deployed location.

Azure AD B2C Configuration
--------------------------

### Create API Policy Keys

Following [this
documentation](https://docs.microsoft.com/en-us/azure/active-directory-b2c/secure-rest-api#add-rest-api-username-and-password-policy-keys),
create two policy keys -- one for the API Username, and one for the API
password which you defined above.

The sample policy uses these key names:

-   B2C\_1A\_RestApiUsername

-   B2C\_1A\_RestApiPassword

### Update API URL

In the provided TrustFrameworkExtensions policy, find the Technical
Profile named "Rest-LexisNexus-SessionQuery", and update the ServiceUrl
metadata item with the location of the API deployed above.

### Update UI URL

In the provided TrustFrameworkExtensions policy, do a find and replace
to search for "https://yourblobstorage/blobcontainer/" with the location
the UI files are deployed to.

### Configure the B2C Policy

Follow [this
documentation](https://docs.microsoft.com/en-us/azure/active-directory-b2c/custom-policy-get-started?tabs=applications#custom-policy-starter-pack)
to configure the policy for the B2C tenant. The provided policies will
need to be updated to relate to your specific tenant.

Notes
=====

This sample policy is based on [LocalAccounts starter
pack](https://github.com/Azure-Samples/active-directory-b2c-custom-policy-starterpack/tree/master/LocalAccounts).
