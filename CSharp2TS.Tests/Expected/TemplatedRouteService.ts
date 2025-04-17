// Auto-generated from TemplatedRouteController.cs

import { useApiClient } from './apiClient';

const { apiClient } = useApiClient();

export default {
  async get(): Promise<string> {
    const response = await apiClient.get<string>(`api/templatedroute`);
    return response.data; 
  },

};
