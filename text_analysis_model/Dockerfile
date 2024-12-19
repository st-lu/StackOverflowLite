# Folosim o imagine oficială de Python ca bază
FROM python:3.11-slim

# Setează directorul de lucru în container
WORKDIR /app

# Copiază fișierele din directorul local în container
COPY . /app

RUN pip install --upgrade pip
RUN pip install --no-cache-dir -r requirements.txt


# Expune portul 5000 pentru a accesa API-ul
EXPOSE 5000

# Comandă pentru a lansa aplicația
CMD ["python3", "/app/main.py"]
