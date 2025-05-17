// auth.api.js
import instance from '@/services/axiosConfig';

// Update a user by ID
const linkAccountApi = async (data) => instance.post(`/api/web/user/link-account`, data);
const updateDisplayNameApi = async (data) => instance.post(`/api/web/user/update-display-name`, data);

// Get a user by ID
const getInforApi = async () => instance.get(`/api/web/user/get-infor`);

export {
  linkAccountApi,
  updateDisplayNameApi,
  getInforApi
};
