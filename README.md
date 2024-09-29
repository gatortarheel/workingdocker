# Copy/paste-ish commands
## for review - you did a bunch of this 2 years ago but lost access due to...

- [ ] Build the project
- [ ] Build the image
- [ ] Run the container
      

`docker image build --pull --file "C:\source\workingdocker/Dockerfile" --tag "workingdocker:latest" --label "com.microsoft.created-by=visual-studio-code" --platform "linux/amd64" "C:\source\workingdocker"`

`docker run --rm -d -p 5000:5000/tcp workingdocker:latest `

---
Source:
https://code.visualstudio.com/docs/containers/quickstart-aspnet-core
