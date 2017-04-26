FROM		busybox:latest

RUN		mkdir /app
COPY		app.sh /app
RUN		touch /app/settings.json

ENTRYPOINT	["/app/app.sh"]
