import instance from '@/services/axiosConfig';

const login = async ({ email, password }) => instance.post('/api/web/auth/login', { email, password });
const saveInformation = async (data) => instance.post('/api/web/auth/google-login', data);

export {
  login,
  saveInformation
}