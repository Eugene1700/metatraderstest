import AbstractView from "./AbstractView.js";

function onSendIp() {
    const ip = document.getElementById('ipAddress').value;
    sendIp(ip);
}

function sendIp(ip) {
    fetch("http://localhost:5000/ip/location?ip="+ip)
        .then(response => {
            if (!response.ok) {
                throw new Error(`Request failed with status ${reponse.status}`)
            }
            return response.json()
        })
        .then(data => {
            console.log(data);
        })
        .catch(error => console.log(error))
}

export default class extends AbstractView {
    constructor(params) {
        super(params);
        this.setTitle("Dashboard");
        window.onload = function() {
            const elem = document.getElementById('sendIp');
            elem.onclick = onSendIp;
        };
    }

    async getHtml() {
        return `
            <h1>Поиск гео-информации по IP</h1>
            <p>
                Введите IP-адрес
            </p>
            <p>
                <input id="ipAddress" type="text"/>
                <input id="sendIp" type="submit" onclick="return sendIp();"/>
            </p>
        `;
    }
}