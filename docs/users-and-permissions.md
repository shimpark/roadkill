# Users and permissions

## Basics
Roadkill has just two types of user roles: editors (or standard users) and admins. The only difference between the two is that admin users who belong to the admin role can access the "site settings" page, delete and lock pages.

## Authentication
By default, Roadkill uses the database to store its users and passwords. For various reasons it does not use the ASP.NET AuthenticationProviders, but its own security system. The system uses the username field for the page history, however the login always uses the email address, and the minimum password length is 6 characters. You can enable or disable user signups as an admin under the "site settings" page. If you disable user signups, then all users will need to be created via the "site settings" page, where you can also set their password. There is no notion of an anonymous user in Roadkill, all edits and new pages need a user login to do so.

If you enable user signups then anybody can create a user (editor) account on the wiki and create and edit pages. You can restrict pages to so they are only editable by admins however, more details on this can be found on the [Creating and editing pages page](creating-and-editing-pages.md).

If your wiki is available to the public via the internet, it's recommended that you enable the Recaptcha option. Recaptcha (http://www.google.com/recaptcha) will go someway to stop spam bots from creating new logins on your wiki. To enable this, you will need to create an account on the Google Recaptcha page, and copy the API key from there into the Roadkill wizard or site settings page.

Roadkill does require new user accounts (if user signups are enabled) to confirm their account via an email address as an extra security measure, so you will need to configure an email server that emails are sent via, in your web.config (see the [configuration](configuration.md) page for details), if you decide to allow user signups.

## Active Directory authentication
If you want to run Roadkill inside a Windows based network that uses Active Directory for user logins, Roadkill supports this out-of-the-box. The install wizard will guide you on how configure the various options available and does include fairly detailed instructions.

It's worth downloading [AD Explorer](http://technet.microsoft.com/en-us/sysinternals/bb963907.aspx) first before configuring the Active Directory options in Roadkill, as it makes it easier to discover the groups and LDAP:// settings required. You are able to set multiple AD groups for the editor and admin roles by separating each group with a comma.

You will need a login that can read from the Active Directory in order for Roadkill to authenticate correctly. This login should ideally be one that does not have a password expiry, but at the same time is not able to login as a desktop user, sometimes known as a service user. Roadkill does not use the application pool login for AD authentication via pass-through auth.

## Writing your own user manager
If you want to use an external data source for authenticating your users, this is fairly straight forward to do by implementing the Roadkill.Core.UserServiceBase class. This is an abstract class and the default FormsAuthUserService is a good place to start for seeing how the database user authentication is implemented.

Once you've written your authentication class, you will need to configure it via the Roadkill [Web.config] settings. The setting is userServiceType, which takes the format userManagerType="Yournamespace.Classname, Yourassemblyname". The type should be placed in the ~/Plugins/ folder where it will be copied to the bin folder on startup, and used.

Fredrik Stolpe has written [this short blog](http://blog.cornbeast.com/2014/03/writing-a-custom-authentication-userservice-for-roadkill-wiki/) post on writing your own user service which gives you more information.

## Adding new users
If you're using the default database user authentication, you can add new users via the admin "site settings" page in Roadkill, which needs an admin login. When you add a user through this interface no email will be sent to the user, and they will be added to the system without needing to confirm their login.

If you are using Active Directory for authentication then you cannot add new users to the system.

## Editing users
As with adding new users, editing users is also done via the "site settings" page. You can use this interface to change people's passwords if needed, which can also be used to de-activate users if needed.

If you are using Active Directory for authentication then you cannot edit users to the system.