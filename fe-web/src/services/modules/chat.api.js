import instance from '@/services/axiosConfig';

// Get all chat messages
const getAllHitoryChatApi = async () => instance.get('api/web/chat/history');
const getDefaultQuestionChatApi = async () => instance.get('api/web/chat/default-questions');
const sendDefaultQuestionChatApi = async (data) => instance.post('api/web/chat/default-questions/answer', data);

const sendMessageApi = async (data) => instance.post('api/web/chat/send', data);
const loadTokenUserApi = async () => instance.get('api/web/chat/token');

export {
    getAllHitoryChatApi,
    sendMessageApi,
    loadTokenUserApi,
    getDefaultQuestionChatApi,
    sendDefaultQuestionChatApi
};