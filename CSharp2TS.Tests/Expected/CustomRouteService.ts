// Auto-generated from CustomRouteController.cs

import { useApiClient } from './apiClient';

const { apiClient } = useApiClient();

export default {
  async get(): Promise<string> {
    const response = await apiClient.get<string>(`api/custom-route`);
    return response.data; 
  },

};
