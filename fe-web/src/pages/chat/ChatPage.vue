<template>
    <NavbarComponentV1 />
    <div class="flex flex-col h-screen bg-gradient-to-b from-gray-50 to-gray-100">
        <!-- Chat Container -->
        <div class="flex-1 overflow-y-auto p-6" ref="chatContainer">
            <div class="max-w-3xl mx-auto space-y-4">
                <div v-for="message in chatHistory" :key="message.id"
                    :class="['flex', message.type === 'request' ? 'justify-end' : 'justify-start']">
                    <div :class="[
                        'max-w-lg p-4 rounded-2xl shadow-md relative group transition-all duration-200',
                        message.type === 'request' 
                            ? 'bg-gradient-to-r from-blue-600 to-blue-700 text-white' 
                            : 'bg-white text-gray-800'
                    ]">
                        <div class="flex items-start gap-4">
                            <img v-if="message.type !== 'request'" 
                                :src="instance.defaults.baseURL + urlImageChatbot"
                                class="w-10 h-10 rounded-full flex-shrink-0 border border-gray-200"
                                alt="Chatbot Avatar">
                            <div class="flex-1">
                                <p class="text-base leading-relaxed">{{ message.content }}</p>
                                <span class="text-xs opacity-60 mt-2 block">
                                    {{ formatTimestamp(message.createdAt) }}
                                </span>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>

        <!-- Input Bar -->
        <div class="bg-white p-6 border-t border-gray-200 sticky bottom-0 shadow-lg">
            <div class="max-w-3xl mx-auto flex gap-4 items-center">
                <div class="flex items-center gap-2 text-sm font-medium text-gray-600">
                    <span class="inline-flex items-center px-3 py-1 bg-gray-100 rounded-full">
                        <svg class="w-4 h-4 mr-1" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                            <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M12 8c-1.657 0-3 .895-3 2s1.343 2 3 2 3 .895 3 2-1.343 2-3 2m0-8c1.11 0 2.08.402 2.599 1M12 8V7m0 1v8m0 0v1m0-1c-1.11 0-2.08-.402-2.599-1M21 12a9 9 0 11-18 0 9 9 0 0118 0z" />
                        </svg>
                        Tokens: {{ tokenUser}}
                    </span>
                </div>
                <input
                    v-model="newMessage"
                    @keyup.enter="sendMessage"
                    type="text"
                    placeholder="Type your message..."
                    class="flex-1 p-4 border border-gray-200 rounded-full bg-gray-50 focus:outline-none focus:ring-2 focus:ring-blue-500 focus:border-transparent transition-all duration-200"
                    :disabled="tokenUser == 0"
                >
                <button
                    @click="sendMessage"
                    :disabled="!newMessage.trim() || tokenUser == 0"
                    class="px-6 py-4 bg-gradient-to-r from-blue-600 to-blue-700 text-white rounded-full disabled:bg-gray-400 disabled:cursor-not-allowed hover:from-blue-700 hover:to-blue-800 transition-all duration-200 font-medium"
                >
                    Send
                </button>
            </div>
            <p v-if="tokenUser == 0" class="text-red-500 text-sm mt-2 text-center">
                Không đủ token. Vui lòng mua thêm token để tiếp tục trò chuyện.
            </p>
        </div>

        <!-- Scroll to Top Button -->
        <button
            v-if="showScrollToTop"
            @click="scrollToTop"
            class="fixed bottom-20 right-6 p-3 bg-blue-600 text-white rounded-full shadow-lg hover:bg-blue-700 transition-all duration-200 transform hover:scale-105"
        >
            <svg class="w-6 h-6" fill="none" stroke="currentColor" viewBox="0 0 24 24">
                <path stroke-linecap="round" stroke-linejoin="round" stroke-width="2" d="M5 15l7-7 7 7" />
            </svg>
        </button>
    </div>
</template>

<script setup>
import { onMounted, ref, computed } from 'vue';
import NavbarComponentV1 from '@/components/NavbarComponentV1.vue';
import {
    getAllHitoryChatApi,
    sendMessageApi,
    loadTokenUserApi
} from '@/services/modules/chat.api';
import instance from '@/services/axiosConfig';

const urlImageChatbot = '/images/avatar-chatbot.png';
const chatHistory = ref([]);
const newMessage = ref('');
const chatContainer = ref(null);
const scrollPosition = ref(0);
const tokenUser = ref(null);

onMounted(async () => {
    await fetchChatHistory();
    await fetchTokenUser();
    chatContainer.value.addEventListener('scroll', handleScroll);
});

const fetchChatHistory = async () => {
    try {
        const response = await getAllHitoryChatApi();
        chatHistory.value = JSON.parse(response.data.data);
    } catch (error) {
        console.error('Error fetching chat history:', error);
    }
};

const fetchTokenUser = async () => {
    try {
        const response = await loadTokenUserApi();
        if (response.data && response.data.data) {
            tokenUser.value = response.data.data.tokenChat;
        } else {
            console.error('No token user found in the response');
        }
    } catch (error) {
        console.error('Error loading token user:', error);
    }
};

const sendMessage = async () => {
    if (!newMessage.value.trim() || tokenUser.value?.token <= 0) return;

    const userMessage = {
        id: Date.now(),
        content: newMessage.value,
        type: 'request',
        createdAt: new Date().toISOString()
    };
    chatHistory.value.push(userMessage);

    try {
        const response = await sendMessageApi({ content: newMessage.value });
        chatHistory.value.push({
            id: response.data.data.id || Date.now(),
            content: response.data.data.response,
            type: 'response',
            createdAt: new Date().toISOString()
        });
        // Update token count after successful message
        await fetchTokenUser();
    } catch (error) {
        if (error.response && error.response.status === 400) {
            chatHistory.value.push({
                id: Date.now(),
                content: error.response.data.message,
                type: 'response',
                createdAt: new Date().toISOString()
            });
        } else {
            chatHistory.value.push({
                id: Date.now(),
                content: 'Error: Could not send message',
                type: 'response',
                createdAt: new Date().toISOString()
            });
        }
    }

    newMessage.value = '';
    // Scroll to bottom
    setTimeout(() => {
        chatContainer.value.scrollTop = chatContainer.value.scrollHeight;
    }, 0);
};

const formatTimestamp = (timestamp) => {
    if (!timestamp || typeof timestamp !== 'string') return 'Invalid date';
    const cleanedTimestamp = timestamp.replace(/\+.*$/, 'Z');
    const date = new Date(cleanedTimestamp);
    if (isNaN(date)) return 'Invalid date';
    return date.toLocaleTimeString([], { hour: '2-digit', minute: '2-digit' }) + ' ' + date.toLocaleDateString();
};

// Scroll to Top Logic
const showScrollToTop = computed(() => scrollPosition.value > 300);

const handleScroll = () => {
    scrollPosition.value = chatContainer.value.scrollTop;
};

const scrollToTop = () => {
    chatContainer.value.scrollTo({ top: 0, behavior: 'smooth' });
};
</script>

<style scoped>
.overflow-y-auto {
    scroll-behavior: smooth;
}

/* Message bubble styling */
.rounded-2xl {
    border-radius: 1.5rem;
}

/* Message bubble tail */
.rounded-2xl::before {
    content: '';
    position: absolute;
    bottom: 0;
    width: 0;
    height: 0;
    border: 0.75rem solid transparent;
}

.bg-gradient-to-r.from-blue-600::before {
    right: -0.75rem;
    border-bottom-color: #2563eb;
    border-right-color: transparent;
}

.bg-white::before {
    left: -0.75rem;
    border-bottom-color: white;
    border-left-color: transparent;
}

/* Input hover and focus effects */
input:focus {
    box-shadow: 0 0 0 4px rgba(59, 130, 246, 0.15);
}

/* Message bubble animations */
.max-w-lg {
    transition: transform 0.3s ease-out, opacity 0.3s ease-out;
    opacity: 0.95;
}

.max-w-lg:hover {
    transform: translateY(-3px);
    opacity: 1;
}

/* Smooth fade-in for new messages */
.max-w-lg {
    animation: fadeIn 0.3s ease-out;
}

@keyframes fadeIn {
    from { opacity: 0; transform: translateY(10px); }
    to { opacity: 0.95; transform: translateY(0); }
}
</style>