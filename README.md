# Papercut

Self-contained run (no external services required):

```bash
docker compose up --build
```

If your Docker installation does not include the `compose` subcommand:

```bash
docker build -t papercut-web:local .
docker run --rm -p 8080:8080 -v papercut-data:/data papercut-web:local
```

App URL:

- http://localhost:8080
- health check: http://localhost:8080/health

Default seeded user (reset on each app start):

- Email: `demo@papercut.local`
- Password: `Papercut!123`
- Default culture: `en-GB`
- Default time zone: `UTC`

Stop and clean up:

```bash
docker compose down
```
