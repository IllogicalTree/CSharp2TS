// Auto-generated from FormController.cs

import { apiClient } from './apiClient';

export default {
  async postForm(form: FormData): Promise<void> {
    await apiClient.instance.post(`api/form`, form, {
      headers: { 'Content-Type': 'multipart/form-data' },
    });
  },

};
