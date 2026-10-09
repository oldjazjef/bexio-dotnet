# Releasing

## CI

`.github/workflows/ci.yml` runs on every push to main and every pull request: restore, build with warnings as errors, tests with coverage, `dotnet pack`. Test results, coverage and the `.nupkg` are uploaded as artifacts.

## Publishing to NuGet

`.github/workflows/release.yml` publishes when a tag `vX.Y.Z` is pushed:

```
git tag v2.1.0
git push origin v2.1.0
```

The version of the package is taken from the tag (a tag like `v2.1.0-beta.1` publishes a pre-release). The workflow builds, tests, packs, pushes to nuget.org and creates a GitHub release.

Publishing uses **NuGet trusted publishing** (OIDC), no API key is stored:

1. nuget.org: Account, Trusted Publishing, add a policy for this repository, workflow file `release.yml` and the environment `production`.
2. GitHub: create the environment `production` and the repository variable `NUGET_USER` (your nuget.org profile name).

A published version cannot be deleted on nuget.org, only unlisted or deprecated.

## Dependencies

Dependabot proposes updates for NuGet packages and GitHub Actions (`.github/dependabot.yml`).
