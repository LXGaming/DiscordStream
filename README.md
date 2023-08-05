# DiscordStream

[![License](https://img.shields.io/github/license/LXGaming/DiscordStream?label=License&cacheSeconds=86400)](https://github.com/LXGaming/DiscordStream/blob/master/LICENSE)

## Reverse Proxy

**Nginx**
```nginx
server {
    ...
    location /webhooks/ {
        include /etc/nginx/proxy_params;
        proxy_pass http://127.0.0.1:8080/api/webhooks/;
        proxy_redirect http://127.0.0.1:8080/api/webhooks/ https://$server_name/webhooks/;
    }
}
```

## License
DiscordStream is licensed under the [Apache 2.0](https://github.com/LXGaming/DiscordStream/blob/master/LICENSE) license.