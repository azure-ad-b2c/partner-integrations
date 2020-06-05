# Azure Active Directory B2C: Partner Integrations

In this repo, you will find samples for integrating partner solutions with Azure AD B2C.

## Getting started
- See our [Azure AD B2C Wiki articles](https://github.com/azure-ad-b2c/ief-wiki/wiki) here to help walkthrough the custom policy components.

- See our Custom Policy Documentation [here](https://aka.ms/ief).

- See our Custom Policy Schema reference [here](https://docs.microsoft.com/en-us/azure/active-directory-b2c/active-directory-b2c-reference-trustframeworks-defined-ief-custom).

## Prerequisites
- You will require to create an Azure AD B2C directory, see the guidance [here](https://docs.microsoft.com/en-us/azure/active-directory-b2c/tutorial-create-tenant).

- To use the sample policies in this repo, follow the instructions here to setup your AAD B2C environment for Custom Policies [here](https://docs.microsoft.com/en-us/azure/active-directory-b2c/active-directory-b2c-get-started-custom).

- For any custom policy sample which makes use of Extension attributes, follow the guidance [here](https://docs.microsoft.com/en-us/azure/active-directory-b2c/active-directory-b2c-create-custom-attributes-profile-edit-custom#create-a-new-application-to-store-the-extension-properties) and [here](https://docs.microsoft.com/en-us/azure/active-directory-b2c/active-directory-b2c-create-custom-attributes-profile-edit-custom#modify-your-custom-policy-to-add-the-applicationobjectid). The `AAD-Common` Technical profile will always need to be modified to use your `ApplicationId` and `ObjectId`.


## Integrations
- [Experian](samples/Experian) - Integrate Azure AD B2C with [Experian](https://www.experian.com/decision-analytics/account-opening-fraud/microsoft-integration).

- [IDology](samples/IDology) - Integrate Azure AD B2C with [IDology](https://www.idology.com/request-a-demo/microsoft-integration-signup/).

- [ThreatMetrix](samples/ThreatMetrix) - Integrate Azure AD B2C with ThreatMetrix.

- [Twilio Verify API for PSD2 SCA](samples/Twilio-VerifyAPI) - The following sample guides you through integrating Azure AD B2C authentication with Twilio Verify API to enable your organization to meet PSD2 SCA requirements.

- [TypingDNA](samples/TypingDNA) - This sample demonstrates how to integrate TypingDNA as a PSD2 SCA compliant authentication factor. Find more about TypingDNA [here](https://www.typingdna.com/).

## Community Help and Support
Use [Stack Overflow](https://stackoverflow.com/questions/tagged/azure-ad-b2c) to get support from the community. Ask your questions on Stack Overflow first and browse existing issues to see if someone has asked your question before. Make sure that your questions or comments are tagged with [azure-ad-b2c].
If you find a bug in the sample, please raise the issue on [GitHub Issues](https://github.com/azure-ad-b2c/samples/issues).
To provide product feedback, visit the Azure Active Directory B2C [Feedback page](https://feedback.azure.com/forums/169401-azure-active-directory?category_id=160596).
