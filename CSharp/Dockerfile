FROM mono:5
EXPOSE 12345
ADD . /src
WORKDIR /src
RUN xbuild GroupFormationAlgorithm.sln
CMD ["mono", "GroupFormationAlgorithm/bin/Debug/GroupFormationAlgorithm.exe"]
