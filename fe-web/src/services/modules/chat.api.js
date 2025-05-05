import instance from '@/services/axiosConfig';

// Get all chat messages
const getAllHitoryChatApi = async () => instance.get('api/web/chat/history');

const sendMessageApi = async (data) => instance.post('api/web/chat/send', data);
const loadTokenUserApi = async () => instance.get('api/web/chat/token');

export {
    getAllHitoryChatApi,
    sendMessageApi,
    loadTokenUserApi
};