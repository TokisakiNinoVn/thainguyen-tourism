import instance from '@/services/axiosConfig';

const login = async ({ email, password }) => instance.post('/api/web/auth/login', { email, password });
const registerApi = async (data) => instance.post('/api/web/auth/register', data);

const saveInformation = async (data) => instance.post('/api/web/auth/first-google-login', data);
const loginGoogleApi = async (data) => instance.post('/api/web/auth/google-login', data);

export {
  login,
  saveInformation,
  registerApi,
  loginGoogleApi
}