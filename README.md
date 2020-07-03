# DiscordStream

[![Build Status](https://api.travis-ci.com/LXGaming/DiscordStream.svg?branch=master)](https://travis-ci.com/LXGaming/DiscordStream)
[![License](https://lxgaming.github.io/badges/License-Apache%202.0-blue.svg)](https://www.apache.org/licenses/LICENSE-2.0)

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
DiscordStream is licensed under the [Apache 2.0](https://www.apache.org/licenses/LICENSE-2.0) license.