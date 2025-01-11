FROM python:3.11-slim

WORKDIR /app

COPY . /app

RUN mkdir -p /app/logs && chmod 777 /app/logs

RUN pip install --upgrade pip
RUN pip install --no-cache-dir -r requirements.txt

EXPOSE 8000

ENV FLASK_APP=main.py
ENV FLASK_ENV=development

CMD ["flask", "run", "--host=0.0.0.0", "--port=8000"]