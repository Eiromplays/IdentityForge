# IdentityForge
<!-- ALL-CONTRIBUTORS-BADGE:START - Do not remove or modify this section -->
[![All Contributors](https://img.shields.io/badge/all_contributors-1-orange.svg?style=flat-square)](#contributors-)
<!-- ALL-CONTRIBUTORS-BADGE:END -->

#### If you run on linux you need to make sure you have a proper dev cert installed if running through docker:
https://github.com/BorisWilhelms/create-dotnet-devcert/tree/main
### if you run on windows or mac you can just run the following command:
```bash
dotnet dev-certs https --trust
```
### And then change the volume mount in the docker-compose file to the following:
```yaml
volumes:
  - ~/.aspnet/https:/https:ro
```
### And the ASPNETCORE_Kestrel__Certificates__Default__Path to the following:
```yaml
ASPNETCORE_Kestrel__Certificates__Default__Path: /https/yourCertNameHere.pfx
```

#

## Contributors ✨

Thanks goes to these wonderful people ([emoji key](https://allcontributors.org/docs/en/emoji-key)):

<!-- ALL-CONTRIBUTORS-LIST:START - Do not remove or modify this section -->
<!-- prettier-ignore-start -->
<!-- markdownlint-disable -->
<table>
  <tr>
    <td align="center"><a href="https://eirik.frozensoftsoftware.com"><img src="https://avatars.githubusercontent.com/u/26148920?v=4?s=100" width="100px;" alt=""/><br /><sub><b>Eirik Sjøløkken</b></sub></a><br /><a href="https://github.com/Eiromplays/IdentityServer.Admin/commits?author=Eiromplays" title="Code">💻</a> <a href="https://github.com/Eiromplays/IdentityServer.Admin/commits?author=Eiromplays" title="Documentation">📖</a> <a href="#design-Eiromplays" title="Design">🎨</a> <a href="#blog-Eiromplays" title="Blogposts">📝</a> <a href="#infra-Eiromplays" title="Infrastructure (Hosting, Build-Tools, etc)">🚇</a> <a href="#maintenance-Eiromplays" title="Maintenance">🚧</a> <a href="#translation-Eiromplays" title="Translation">🌍</a> <a href="#tutorial-Eiromplays" title="Tutorials">✅</a> <a href="#security-Eiromplays" title="Security">🛡️</a> <a href="https://github.com/Eiromplays/IdentityServer.Admin/pulls?q=is%3Apr+reviewed-by%3AEiromplays" title="Reviewed Pull Requests">👀</a></td>
  </tr>
</table>

<!-- markdownlint-restore -->
<!-- prettier-ignore-end -->

<!-- ALL-CONTRIBUTORS-LIST:END -->

This project follows the [all-contributors](https://github.com/all-contributors/all-contributors) specification. Contributions of any kind welcome!