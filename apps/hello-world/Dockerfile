FROM ubuntu:16.04
RUN apt-get update
RUN apt-get install -y nginx
ADD config/nginx.conf /etc/nginx/nginx.conf

ADD ./src /www-data

EXPOSE 80
CMD ["nginx"]