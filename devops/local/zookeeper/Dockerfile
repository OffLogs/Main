FROM ubuntu:focal

WORKDIR /app

# Change timezone to local time
ENV TZ=UTC
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone

RUN apt-get update
RUN apt-get install -y default-jre

COPY ./binable/kafka /app

COPY ./zookeeper/boot.sh /usr/local/bin/app_boot
CMD ["/usr/local/bin/app_boot"]