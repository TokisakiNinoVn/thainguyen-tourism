<template>
  <div
    v-if="isOpen"
    class="fixed inset-0 bg-black bg-opacity-50 z-50 transition-opacity duration-300"
    @click.self="closePopup"
  >
    <div
      class="bg-white rounded-2xl shadow-2xl flex flex-col w-full max-w-3xl h-[85vh] md:h-[80vh] fixed bottom-8 right-8 overflow-hidden transform transition-all duration-300"
    >
      <!-- Header with Close Button -->
      <div
        class="bg-gray-100 p-4 flex justify-between items-center border-b border-gray-200"
      >
        <h2 class="text-xl font-semibold text-gray-800">Chatbot</h2>
        <button
          @click="closePopup"
          class="p-2 rounded-full hover:bg-gray-200 transition-all duration-200"
        >
          <svg
            class="w-6 h-6 text-gray-600"
            fill="none"
            stroke="currentColor"
            viewBox="0 0 24 24"
          >
            <path
              stroke-linecap="round"
              stroke-linejoin="round"
              stroke-width="2"
              d="M6 18L18 6M6 6l12 12"
            />
          </svg>
        </button>
      </div>

      <!-- Chat Container -->
      <div class="flex-1 overflow-y-auto p-6" ref="chatContainer">
        <div class="space-y-4">
          <div
            v-for="message in chatHistory"
            :key="message.id"
            :class="[
              'flex',
              message.type === 'request' ? 'justify-end' : 'justify-start',
            ]"
          >
            <div
              :class="[
                'max-w-lg p-4 rounded-2xl shadow-sm relative group transition-all duration-300',
                message.type === 'request'
                  ? 'bg-gradient-to-r from-blue-500 to-blue-600 text-white'
                  : 'bg-gray-100 text-gray-900',
              ]"
            >
              <div class="flex items-start gap-3">
                <img
                  v-if="message.type !== ' salience request'"
                  :src="instance.defaults.baseURL + urlImageChatbot"
                  class="w-10 h-10 rounded-full flex-shrink-0 border border-gray-200"
                  alt="Chatbot Avatar"
                />
                <div class="flex-1">
                  <p class="text-base leading-relaxed">{{ message.content }}</p>
                  <span class="text-xs opacity-70 mt-2 block">
                    {{ formatTimestamp(message.createdAt) }}
                  </span>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>

      <!-- Default Questions -->
      <div
        class="bg-gray-50 p-4 border-t border-gray-200 transition-all duration-300"
        :class="showDefaultQuestions ? 'block' : 'hidden'"
      >
        <div class="flex items-center justify-between mb-3">
          <h3 class="text-sm font-semibold text-gray-700">Câu hỏi gợi ý</h3>
          <button
            @click="toggleDefaultQuestions"
            class="p-1.5 rounded-full hover:bg-gray-200 transition-all duration-200"
          >
            <svg
              class="w-4 h-4 text-gray-600 transform transition-transform duration-200"
              :class="showDefaultQuestions ? 'rotate-180' : ''"
              fill="none"
              stroke="currentColor"
              viewBox="0 0 24 24"
            >
              <path
                stroke-linecap="round"
                stroke-linejoin="round"
                stroke-width="2"
                d="M19 9l-7 7-7-7"
              />
            </svg>
          </button>
        </div>
        <div class="flex flex-wrap gap-2">
          <button
            v-for="question in defaultQuestion"
            :key="question.id"
            @click="selectDefaultQuestion(question)"
            class="px-4 py-1.5 bg-blue-50 text-blue-700 rounded-full text-xs font-medium hover:bg-blue-100 hover:shadow-md transition-all duration-200 transform hover:scale-105"
          >
            {{ question.question }}
          </button>
        </div>
      </div>

      <!-- Input Bar -->
      <div class="bg-white p-4 border-t border-gray-200">
        <div class="flex gap-3 items-center">
          <div
            class="flex items-center gap-2 text-xs font-medium text-gray-600"
          >
            <button
              @click="toggleDefaultQuestions"
              class="p-1.5 rounded-full bg-gray-100 hover:bg-gray-200 transition-all duration-200"
            >
              <svg
                class="w-4 h-4 text-gray-600"
                fill="none"
                stroke="currentColor"
                viewBox="0 0 24 24"
              >
                <path
                  stroke-linecap="round"
                  stroke-linejoin="round"
                  stroke-width="2"
                  d="M8.228 9c.549-1.165 2.03-2 3.772-2 2.21 0 4 1.343 4 3 0 1.4-1.278 2.575-3.006 2.907-.542.104-.994.54-.994 1.093m0 3h.01M21 12a9 9 0 11-18 0 9 9 0 0118 0z"
                />
              </svg>
            </button>
            <span
              class="inline-flex items-center px-3 py-1 bg-gray-100 rounded-full"
            >
              <svg
                class="w-4 h-4 mr-1"
                fill="none"
                stroke="currentColor"
                viewBox="0 0 24 24"
              >
                <path
                  stroke-linecap="round"
                  stroke-linejoin="round"
                  stroke-width="2"
                  d="M12 8c-1.657 0-3 .895-3 2s1.343 2 3 2 3 .895 3 2-1.343 2-3 2m0-8c1.11 0 2.08.402 2.599 1M12 8V7m0 1v8m0 0v1m0-1c-1.11 0-2.08-.402-2.599-1M21 12a9 9 0 11-18 0 9 9 0 0118 0z"
                />
              </svg>
              Tokens: {{ tokenUser }}
            </span>
          </div>
          <input
            v-model="newMessage"
            @keyup.enter="sendMessage"
            type="text"
            placeholder="Nhập tin nhắn của bạn..."
            class="flex-1 p-3 border border-gray-200 rounded-full bg-gray-50 text-base focus:outline-none focus:ring-2 focus:ring-blue-400 focus:border-transparent transition-all duration-200"
            :disabled="tokenUser == 0"
          />
          <button
            @click="sendMessage"
            :disabled="!newMessage.trim() || tokenUser == 0"
            class="px-6 py-3 bg-gradient-to-r from-blue-500 to-blue-600 text-white rounded-full disabled:bg-gray-300 disabled:cursor-not-allowed hover:from-blue-600 hover:to-blue-700 transition-all duration-200 font-semibold shadow-md"
          >
            Gửi
          </button>
        </div>
        <p
          v-if="tokenUser == 0"
          class="text-red-500 text-xs mt-2 text-center font-medium"
        >
          Không đủ token. Vui lòng mua thêm token để tiếp tục trò chuyện.
        </p>
      </div>

      <!-- Scroll to Top Button -->
      <button
        v-if="showScrollToTop"
        @click="scrollToTop"
        class="absolute bottom-20 right-4 p-3 bg-blue-500 text-white rounded-full shadow-lg hover:bg-blue-600 transition-all duration-200 transform hover:scale-110"
        title="Cuộn lên đầu"
      >
        <svg
          class="w-5 h-5"
          fill="none"
          stroke="currentColor"
          viewBox="0 0 24 24"
        >
          <path
            stroke-linecap="round"
            stroke-linejoin="round"
            stroke-width="2"
            d="M5 15l7-7 7 7"
          />
        </svg>
      </button>
    </div>
  </div>
</template>

<script setup>
import { ref, computed, nextTick, watch, defineProps, defineEmits } from "vue";
import {
  getAllHitoryChatApi,
  sendMessageApi,
  loadTokenUserApi,
  getDefaultQuestionChatApi,
  sendDefaultQuestionChatApi,
} from "@/services/modules/chat.api";
import instance from "@/services/axiosConfig";

const props = defineProps({
  isOpen: {
    type: Boolean,
    default: false,
  },
});

const emit = defineEmits(["update:isOpen"]);

const urlImageChatbot = "/images/avatar-chatbot.png";
const chatHistory = ref([]);
const newMessage = ref("");
const chatContainer = ref(null);
const scrollPosition = ref(0);
const tokenUser = ref(null);
const defaultQuestion = ref([]);
const showDefaultQuestions = ref(true);

const closePopup = () => {
  emit("update:isOpen", false);
};

const scrollToBottom = async () => {
  await nextTick();
  if (chatContainer.value) {
    chatContainer.value.scrollTop = chatContainer.value.scrollHeight;
  }
};

// Load data when isOpen changes to true
watch(
  () => props.isOpen,
  async (newValue) => {
    if (newValue) {
      await fetchChatHistory();
      await fetchTokenUser();
      await fetchDefaultQuestion();
      await scrollToBottom();
      if (chatContainer.value) {
        chatContainer.value.addEventListener("scroll", handleScroll);
      }
    }
  }
);

const fetchDefaultQuestion = async () => {
  try {
    const response = await getDefaultQuestionChatApi();
    defaultQuestion.value = response.data.data;
  } catch (error) {
    console.error("Error fetching default questions:", error);
  }
};

const fetchChatHistory = async () => {
  try {
    const response = await getAllHitoryChatApi();
    chatHistory.value = JSON.parse(response.data.data);
    await scrollToBottom();
  } catch (error) {
    console.error("Error fetching chat history:", error);
  }
};

const fetchTokenUser = async () => {
  try {
    const response = await loadTokenUserApi();
    if (response.data && response.data.data) {
      tokenUser.value = response.data.data.tokenChat;
    } else {
      console.error("No token user found in the response");
    }
  } catch (error) {
    console.error("Error loading token user:", error);
  }
};

const toggleDefaultQuestions = () => {
  showDefaultQuestions.value = !showDefaultQuestions.value;
};

const selectDefaultQuestion = async (question) => {
  if (tokenUser.value?.token <= 0) return;

  const userMessage = {
    id: Date.now(),
    content: question.question,
    type: "request",
    createdAt: new Date().toISOString(),
  };
  chatHistory.value.push(userMessage);

  const payload = {
    id: question.id,
    content: question.question,
  };

  try {
    const response = await sendDefaultQuestionChatApi(payload);
    chatHistory.value.push({
      id: response.data.data.id || Date.now(),
      content: response.data.data.response,
      type: "response",
      createdAt: new Date().toISOString(),
    });
    await fetchTokenUser();
    await scrollToBottom();
  } catch (error) {
    if (error.response && error.response.status === 400) {
      chatHistory.value.push({
        id: Date.now(),
        content: error.response.data.message,
        type: "response",
        createdAt: new Date().toISOString(),
      });
    } else {
      chatHistory.value.push({
        id: Date.now(),
        content: "Error: Could not send default question",
        type: "response",
        createdAt: new Date().toISOString(),
      });
    }
    await scrollToBottom();
  }
};

const sendMessage = async () => {
  if (!newMessage.value.trim() || tokenUser.value?.token <= 0) return;

  const userMessage = {
    id: Date.now(),
    content: newMessage.value,
    type: "request",
    createdAt: new Date().toISOString(),
  };
  chatHistory.value.push(userMessage);

  try {
    const response = await sendMessageApi({ content: newMessage.value });
    chatHistory.value.push({
      id: response.data.data.id || Date.now(),
      content: response.data.data.response,
      type: "response",
      createdAt: new Date().toISOString(),
    });
    await fetchTokenUser();
    await scrollToBottom();
  } catch (error) {
    if (error.response && error.response.status === 400) {
      chatHistory.value.push({
        id: Date.now(),
        content: error.response.data.message,
        type: "response",
        createdAt: new Date().toISOString(),
      });
    } else {
      chatHistory.value.push({
        id: Date.now(),
        content: "Error: Could not send message",
        type: "response",
        createdAt: new Date().toISOString(),
      });
    }
    await scrollToBottom();
  }

  newMessage.value = "";
};

const formatTimestamp = (timestamp) => {
  if (!timestamp || typeof timestamp !== "string") return "Invalid date";
  const cleanedTimestamp = timestamp.replace(/\+.*$/, "Z");
  const date = new Date(cleanedTimestamp);
  if (isNaN(date)) return "Invalid date";
  return (
    date.toLocaleTimeString([], { hour: "2-digit", minute: "2-digit" }) +
    " " +
    date.toLocaleDateString()
  );
};

const showScrollToTop = computed(() => scrollPosition.value > 300);

const handleScroll = () => {
  if (chatContainer.value) {
    scrollPosition.value = chatContainer.value.scrollTop;
  }
};

const scrollToTop = () => {
  if (chatContainer.value) {
    chatContainer.value.scrollTo({ top: 0, behavior: "smooth" });
  }
};
</script>

<style scoped>
.overflow-y-auto {
  scroll-behavior: smooth;
}

.rounded-2xl {
  border-radius: 1.25rem;
}

.rounded-2xl::before {
  content: "";
  position: absolute;
  bottom: 0;
  width: 0;
  height: 0;
  border: 0.75rem solid transparent;
}

.bg-gradient-to-r.from-blue-500::before {
  right: -0.75rem;
  border-bottom-color: #3b82f6;
  border-right-color: transparent;
}

.bg-gray-100::before {
  left: -0.75rem;
  border-bottom-color: #f3f4f6;
  border-left-color: transparent;
}

input:focus {
  box-shadow: 0 0 0 3px rgba(59, 130, 246, 0.1);
}

.max-w-lg {
  transition: transform 0.3s ease-out, opacity 0.3s ease-out;
  opacity: 0.97;
}

.max-w-lg:hover {
  transform: translateY(-3px);
  opacity: 1;
}

.max-w-lg {
  animation: fadeIn 0.3s ease-out;
}

@keyframes fadeIn {
  from {
    opacity: 0;
    transform: translateY(10px);
  }
  to {
    opacity: 0.97;
    transform: translateY(0);
  }
}

/* Responsive adjustments for smaller screens */
@media (max-width: 640px) {
  .max-w-3xl {
    width: 90%;
    max-width: none;
    bottom: 4;
    right: 4;
  }
}
</style>
