# DiscordStream

[![License](https://lxgaming.github.io/badges/License-Apache%202.0-blue.svg)](https://www.apache.org/licenses/LICENSE-2.0)
[![Patreon](https://lxgaming.github.io/badges/Patreon-donate-yellow.svg)](https://www.patreon.com/lxgaming)
[![Paypal](https://lxgaming.github.io/badges/Paypal-donate-yellow.svg)](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=CZUUA6LE7YS44&item_name=DiscordStream+(from+GitHub.com))

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