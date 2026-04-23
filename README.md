# Ad-platform Backend

## Local Development Setup

### HTTP Client Authentication

The project includes `.http` files in the `API/` directory for testing endpoints. These files require an authentication token, which is automatically fetched using the "Password Grant" flow in development.

To use these files, you must create a private environment file to store your development credentials.

1. Create a file named `API/http-client.private.env.json`.
2. Add the following content:

```json
{
  "development": {
    "devPassword": "Pass1234!",
    "clientSecret": "dev-secret-change"
  }
}
```

*Note: `devPassword` is the password for the seeded `joe@egmrc.com` user, and `clientSecret` must match the value in `Auth/appsettings.Development.json`.*

This file is ignored by Git to prevent accidental exposure of credentials.
