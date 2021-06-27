FROM ubuntu:focal

WORKDIR /app
ENV KAFKA_HOME /app
ENV PATH ${PATH}:${KAFKA_HOME}/bin

# Change timezone to local time
ENV TZ=UTC
RUN ln -snf /usr/share/zoneinfo/$TZ /etc/localtime && echo $TZ > /etc/timezone

RUN apt-get update
RUN apt-get install -y default-jre

RUN echo "Files to copy: "
RUN ls ./binable/kafka
COPY ./binable/kafka /app
RUN chmod -R 700 $KAFKA_HOME

COPY ./kafka/boot.sh /usr/local/bin/app_boot
CMD ["bash", "/usr/local/bin/app_boot"]