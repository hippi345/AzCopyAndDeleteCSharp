# Security Policy

## Supported versions

Security fixes are applied on the `master` branch for the current .NET 8 release of this tool.

## Reporting a vulnerability

If you discover a security issue, please open a private security advisory on GitHub or contact the repository owner. Do not open a public issue for undisclosed vulnerabilities.

## Secrets and credentials

- Never commit storage account keys, connection strings, or SAS tokens to the repository.
- Use environment variables (see `README.md`) or a local secret store.
- If a credential was ever committed to git history, rotate it in Azure Portal immediately and treat the old value as compromised.
