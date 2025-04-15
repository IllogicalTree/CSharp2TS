// this line is ignored in tests

import { useApiClient } from './apiClient';
import TestClass from '../TestClass';

const { apiClient } = useApiClient();

export default {
  async get(): Promise<string> {
    var response = await apiClient.get<string>(`api/Test`);
    return response.data; 
  },

  async getArray(): Promise<string[]> {
    var response = await apiClient.get<string[]>(`api/Test`);
    return response.data; 
  },

  async getList(): Promise<string[]> {
    var response = await apiClient.get<string[]>(`api/Test`);
    return response.data; 
  },

  async get2(id: number): Promise<TestClass> {
    var response = await apiClient.get<TestClass>(`api/Test/${id}`);
    return response.data; 
  },

  async get3(id: number, externalId: number): Promise<TestClass> {
    var response = await apiClient.get<TestClass>(`api/Test/${id}?externalId=${externalId}`);
    return response.data; 
  },

  async getFiltered(filter: string, limit: number): Promise<TestClass> {
    var response = await apiClient.get<TestClass>(`api/Test/filtered?filter=${filter}&limit=${limit}`);
    return response.data; 
  },

  async create(testClass: TestClass): Promise<TestClass> {
    var response = await apiClient.post<TestClass>(`api/Test`, testClass);
    return response.data; 
  },

  async createFromBody(model: string): Promise<string> {
    var response = await apiClient.post<string>(`api/Test`, model);
    return response.data; 
  },

  async update(id: number, testClass: TestClass): Promise<TestClass> {
    var response = await apiClient.put<TestClass>(`api/Test/${id}`, testClass);
    return response.data; 
  },

  async partialUpdate(id: number, model: TestClass): Promise<TestClass> {
    var response = await apiClient.patch<TestClass>(`api/Test/${id}`, model);
    return response.data; 
  },

  async delete(id: number): Promise<void> {
    await apiClient.delete(`api/Test/${id}`);
  },

};

