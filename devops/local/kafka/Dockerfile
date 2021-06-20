FROM ubuntu:focal

WORKDIR /app
ENV KAFKA_HOME /app
ENV PATH ${PATH}:${KAFKA_HOME}/bin

# Change timezone to local time
ENV TZ=UTC
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone

RUN apt-get update
RUN apt-get install -y default-jre

COPY ./binable/kafka_2.13-2.8.0 /app

COPY ./kafka/boot.sh /usr/local/bin/app_boot
CMD ["sh", "/usr/local/bin/app_boot"]