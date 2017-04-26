FROM		busybox:latest

RUN		mkdir /app
COPY		app.sh /app
RUN		echo '{"content": "Some content ...."}' > /app/settings.json

ENTRYPOINT	["/app/app.sh"]
