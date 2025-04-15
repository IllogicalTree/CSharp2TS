// Auto-generated from TemplatedRouteController.cs

import { useApiClient } from './apiClient';

const { apiClient } = useApiClient();

export default {
  async get(): Promise<string> {
    var response = await apiClient.get<string>(`api/templatedroute`);
    return response.data; 
  },

};

