import { ApiRepository } from './api-respository.js';

export class PersonalInfoApiRepository extends ApiRepository {

    constructor() {

        // API Controller Name.
        super('demo');
    }

    async getAll(): Promise<Array<any>> {
        const response = await this.sendRequest('GET', 'getAll');
        return response;
    }

    async add(info: any) {

        const response = await this.sendRequest('POST', 'add', info);
        return response;
    }

    async update(info: any) {

        const response = await this.sendRequest('PUT', 'update', info);
        return response;
    }

    async delete(id: number) {

        const response = await this.sendRequest('DELETE', 'delete/' + id);
        return response;
    }
}