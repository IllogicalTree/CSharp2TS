// Auto-generated from TestController_ActionResult.cs

import { useApiClient } from './apiClient';
import TestClass from '../TestClass';

const { apiClient } = useApiClient();

export default {
  async get(): Promise<string> {
    var response = await apiClient.get<string>(`api/TestController`);
    return response.data; 
  },

  async get2(id: number): Promise<TestClass> {
    var response = await apiClient.get<TestClass>(`api/TestController/${id}`);
    return response.data; 
  },

  async getFiltered(filter: string, limit: number): Promise<TestClass> {
    var response = await apiClient.get<TestClass>(`api/TestController/filtered?filter=${filter}&limit=${limit}`);
    return response.data; 
  },

  async create(testClass: TestClass): Promise<TestClass> {
    var response = await apiClient.post<TestClass>(`api/TestController`, testClass);
    return response.data; 
  },

  async createFromBody(model: string): Promise<string> {
    var response = await apiClient.post<string>(`api/TestController`, model);
    return response.data; 
  },

  async update(id: number, testClass: TestClass): Promise<TestClass> {
    var response = await apiClient.put<TestClass>(`api/TestController/${id}`, testClass);
    return response.data; 
  },

  async partialUpdate(id: number, model: TestClass): Promise<TestClass> {
    var response = await apiClient.patch<TestClass>(`api/TestController/${id}`, model);
    return response.data; 
  },

  async delete(id: number): Promise<void> {
    await apiClient.delete(`api/TestController/${id}`);
  },

};

