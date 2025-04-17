// Auto-generated from NoRouteController.cs

import { useApiClient } from './apiClient';

const { apiClient } = useApiClient();

export default {
  async get(): Promise<string> {
    var response = await apiClient.get<string>(`noroute`);
    return response.data; 
  },

};
