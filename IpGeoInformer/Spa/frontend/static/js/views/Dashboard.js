import AbstractView from "./AbstractView.js";

export default class extends AbstractView {
    constructor(params) {
        super(params);
        this.setTitle("Dashboard");
    }

    async getHtml() {
        return `
            <h1>Поиск гео-информации по IP</h1>
            <p>
                Введите IP-адрес
            </p>
            <p>
                <input id="ipAddress" type="text"/>
                <input id="sendIp" type="submit"/>
            </p>
        `;
    }
}