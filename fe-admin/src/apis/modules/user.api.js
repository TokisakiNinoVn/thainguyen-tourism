import instance from '@/apis/axiosConfig';

const getListUser = async () => instance.get('/api/admin/user/list');
const lockUser = async (id) => instance.put(`/api/admin/user/lock/${id}`);
const unlockUser = async (id) => instance.put(`/api/admin/user/unlock/${id}`);

export {
    getListUser,
    lockUser,
    unlockUser,
};