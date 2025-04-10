import instance from '@/apis/axiosConfig';

const login = async ({ email, password }) => instance.post('/api/admin/auth/login', { email, password });
// check auth: JWT token
const checkAuth = async () => instance.get('/api/admin/auth/check-auth');

export {
    login,
    checkAuth,
};