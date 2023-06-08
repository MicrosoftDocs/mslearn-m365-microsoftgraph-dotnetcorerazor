## .NET Core Razor Pages with Microsoft Graph

This repository is used with the [**Explore Microsoft Graph scenarios for ASP.NET Core development**](https://docs.microsoft.com/learn/paths/m365-msgraph-dotnetcore-scenarios) learning path on Microsoft Learn. To get started, visit the learning path and select one of the modules.

## General Setup Steps to Run the End Solution

See the [Microsoft Learn learning path](https://learn.microsoft.com/training/paths/m365-msgraph-dotnet-core-scenarios) mentioned above for full details.

1. Ensure you have .NET 6+ installed on your machine. You can download and install it from the following link:

    https://dot.net

1. Create a Microsoft 365 developer tenant if you don't already have one:

    https://developer.microsoft.com/microsoft-365/dev-program

    You can view a video that covers key tips here:

    https://www.youtube.com/watch?v=DhhpJ1UjbJ0

1. Register a new app in Azure Active Directory:

    - Login to the Azure Portal.
    - Select Azure Active Directory.
    - Select `App registrations` in the `Manage` section.
    - Select `New registration` in the toolbar.
    - Give the app a name.
    - Select `Accounts in any organizational directory (Any Azure AD directory - Multitenant)` in the `Supported account types`.
    - In the Redirect URI section select `Web` and enter the following URL:

        https://localhost:5001

    - After the app registration is created, note the `clientId` value shown (you'll use it later) .
    - Click the `Authentication` option on the left.
    - Add the following URL into the `Web` section's `Redirect URIs`:

        https://localhost:5001/signin-oidc

    - Add the following URL into the `Front-channel logout URL` section:

        https://localhost:5001:signout-oidc

    - Check the `ID tokens` checkbox.
    - Save your changes.
    - Click `Certificates & secrets` and create a new client secret. Ensure that you copy and store the secret somewhere since this is the only time you'll be able to access it. You'll need it in the next step.

1. Open a terminal window at the root of the `End` folder and run the following commands, substituting `YOUR_APP_ID` with your `Application (client) ID` from the Azure portal, and `YOUR_APP_SECRET` with the application secret you created. 

```
dotnet user-secrets init 
dotnet user-secrets set "AzureAd:ClientId" "YOUR_APP_ID" 
dotnet user-secrets set "AzureAd:ClientSecret" "YOUR_APP_SECRET"
```

1. Run `dotnet restore`
1. Run `dotnet build`
1. Run `dotnet run`

1. Once the app is running, navigate to https://localhost:5001 and login using one of your Microsoft 365 tenant users.
1. Once you're logged in you should see your user name displayed. Click on the menu items at the top to view the user's email, calendar, and files (you may need to add mail messages, calendar events, and files for the user if you don't see any).

NOTE: If you get an SSL certificate error, you can generate a dev certificate for your machine using the following command:

```dotnet dev-certs https -t```

# Contributing

This project welcomes contributions and suggestions.  Most contributions require you to agree to a
Contributor License Agreement (CLA) declaring that you have the right to, and actually do, grant us
the rights to use your contribution. For details, visit https://cla.opensource.microsoft.com.

When you submit a pull request, a CLA bot will automatically determine whether you need to provide
a CLA and decorate the PR appropriately (e.g., status check, comment). Simply follow the instructions
provided by the bot. You will only need to do this once across all repos using our CLA.

This project has adopted the [Microsoft Open Source Code of Conduct](https://opensource.microsoft.com/codeofconduct/).
For more information see the [Code of Conduct FAQ](https://opensource.microsoft.com/codeofconduct/faq/) or
contact [opencode@microsoft.com](mailto:opencode@microsoft.com) with any additional questions or comments.

# Legal Notices

Microsoft and any contributors grant you a license to the Microsoft documentation and other content
in this repository under the [Creative Commons Attribution 4.0 International Public License](https://creativecommons.org/licenses/by/4.0/legalcode),
see the [LICENSE](LICENSE) file, and grant you a license to any code in the repository under the [MIT License](https://opensource.org/licenses/MIT), see the
[LICENSE-CODE](LICENSE-CODE) file.

Microsoft, Windows, Microsoft Azure and/or other Microsoft products and services referenced in the documentation
may be either trademarks or registered trademarks of Microsoft in the United States and/or other countries.
The licenses for this project do not grant you rights to use any Microsoft names, logos, or trademarks.
Microsoft's general trademark guidelines can be found at http://go.microsoft.com/fwlink/?LinkID=254653.

Privacy information can be found at https://privacy.microsoft.com

Microsoft and any contributors reserve all other rights, whether under their respective copyrights, patents,
or trademarks, whether by implication, estoppel or otherwise.
