export abstract class ApiRepository {

    controllerName: string;
    baseUrl: string;

    baseHeader = {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${sessionStorage.getItem('bearer-token')}`,
    };

    constructor(controllerName: string) {
        this.controllerName = controllerName;
        this.baseUrl = `https://${location.host}/api/${controllerName}/`;
    }

    protected async sendRequest(method: string, controllerAction: string, body: any = null): Promise<any> {

        const requestInit: RequestInit = {
            headers: this.baseHeader,
            method: method
        };

        if (body) {
            requestInit.body = JSON.stringify(body);
        }

        const request = await fetch(this.baseUrl + controllerAction, requestInit);
        const response = await request.json();

        return response;
    }
}