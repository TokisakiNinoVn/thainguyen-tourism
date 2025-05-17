<template>
  <NavbarComponentV1 />
  <div
    class="relative flex flex-col h-screen bg-cover bg-center"
    style="
      background-image: url('https://toquoc.mediacdn.vn/Uploaded/minhkhanh/2018_08_19/chef/BINM4708_AOAV.jpg');
    "
  >
    <!-- Overlay for readability -->
    <div class="absolute inset-0 bg-black/50"></div>

    <!-- Chat Container -->
    <div
      class="relative flex-1 overflow-y-auto p-6 md:p-10"
      ref="chatContainer"
    >
      <div class="max-w-4xl mx-auto space-y-6">
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
              'max-w-2xl p-6 rounded-3xl shadow-md relative group transition-all duration-300 animate-fadeIn',
              message.type === 'request'
                ? 'bg-gradient-to-r from-green-500 to-green-600 text-white'
                : 'bg-white/95 backdrop-blur-sm text-gray-900',
            ]"
          >
            <div class="flex items-start gap-4">
              <img
                v-if="message.type !== 'request'"
                :src="instance.defaults.baseURL + urlImageChatbot"
                class="w-10 h-10 rounded-full flex-shrink-0 border border-gray-200 shadow-sm"
                alt="Chatbot Avatar"
              />
              <div class="flex-1">
                <p class="text-base md:text-lg leading-relaxed">
                  {{ message.content }}
                </p>
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
      class="relative bg-green-50/95 backdrop-blur-sm p-6 border-t border-green-200 transition-all duration-300"
      :class="showDefaultQuestions ? 'block' : 'hidden'"
    >
      <div class="max-w-4xl mx-auto">
        <div class="flex items-center justify-between mb-4">
          <h3 class="text-base font-semibold text-green-800">Câu Hỏi Gợi Ý</h3>
          <button
            @click="toggleDefaultQuestions"
            class="p-2 rounded-full hover:bg-green-100 transition-all duration-200"
          >
            <svg
              class="w-5 h-5 text-green-600 transform transition-transform duration-200"
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
        <div class="flex flex-wrap gap-3">
          <button
            v-for="question in defaultQuestion"
            :key="question.id"
            @click="selectDefaultQuestion(question)"
            class="px-5 py-2 bg-green-100 text-green-700 rounded-full text-sm font-medium hover:bg-green-200 hover:shadow-md transition-all duration-200 transform hover:scale-105"
          >
            {{ question.question }}
          </button>
        </div>
      </div>
    </div>

    <!-- Input Bar -->
    <div
      class="relative bg-white/95 backdrop-blur-sm p-6 md:p-8 border-t border-green-200 bottom-0 shadow-2xl"
    >
      <div class="max-w-4xl mx-auto flex gap-4 items-center">
        <div class="flex items-center gap-3 text-sm font-medium text-gray-600">
          <button
            @click="toggleDefaultQuestions"
            class="p-2 rounded-full bg-green-50 hover:bg-green-100 transition-all duration-200"
          >
            <svg
              class="w-5 h-5 text-green-600"
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
            class="inline-flex items-center px-4 py-2 bg-green-50 rounded-full"
          >
            <svg
              class="w-5 h-5 mr-2 text-green-600"
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
          placeholder="Nhập câu hỏi về du lịch Thái Nguyên..."
          class="flex-1 p-4 pl-12 border border-gray-200 rounded-full bg-gray-50 text-base focus:outline-none focus:ring-2 focus:ring-green-500 focus:border-transparent transition-all duration-200"
          :disabled="tokenUser == 0"
        />
        <button
          @click="sendMessage"
          :disabled="!newMessage.trim() || tokenUser == 0"
          class="px-8 py-4 bg-gradient-to-r from-green-500 to-green-600 text-white rounded-full disabled:bg-gray-300 disabled:cursor-not-allowed hover:from-green-600 hover:to-green-700 transition-all duration-200 font-semibold shadow-md transform hover:-translate-y-1"
        >
          Gửi
        </button>
      </div>
      <p
        v-if="tokenUser == 0"
        class="text-red-500 text-sm mt-3 text-center font-medium animate-pulse"
      >
        Không đủ token. Vui lòng mua thêm token để tiếp tục trò chuyện.
      </p>
    </div>

    <!-- Scroll to Top Button -->
    <button
      v-if="showScrollToTop"
      @click="scrollToTop"
      class="fixed bottom-28 right-8 p-4 bg-green-500 text-white rounded-full shadow-lg hover:bg-green-600 transition-all duration-200 transform hover:scale-110"
      title="Cuộn lên đầu"
    >
      <svg
        class="w-6 h-6"
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
</template>

<script setup>
import { onMounted, ref, computed, nextTick } from "vue";
import NavbarComponentV1 from "@/components/NavbarComponentV1.vue";
import {
  getAllHitoryChatApi,
  sendMessageApi,
  loadTokenUserApi,
  getDefaultQuestionChatApi,
  sendDefaultQuestionChatApi,
} from "@/services/modules/chat.api";
import instance from "@/services/axiosConfig";

const urlImageChatbot = "/images/avatar-chatbot.png";
const chatHistory = ref([]);
const newMessage = ref("");
const chatContainer = ref(null);
const scrollPosition = ref(0);
const tokenUser = ref(null);
const defaultQuestion = ref([]);
const showDefaultQuestions = ref(true);

const scrollToBottom = async () => {
  await nextTick();
  if (chatContainer.value) {
    chatContainer.value.scrollTop = chatContainer.value.scrollHeight;
  }
};

onMounted(async () => {
  await fetchChatHistory();
  await fetchTokenUser();
  await fetchDefaultQuestion();
  await scrollToBottom();
  if (chatContainer.value) {
    chatContainer.value.addEventListener("scroll", handleScroll);
  }
});

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
        content: "Lỗi: Không thể gửi câu hỏi gợi ý",
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
        content: "Lỗi: Không thể gửi tin nhắn",
        type: "response",
        createdAt: new Date().toISOString(),
      });
    }
    await scrollToBottom();
  }

  newMessage.value = "";
};

const formatTimestamp = (timestamp) => {
  if (!timestamp || typeof timestamp !== "string") return "Ngày không hợp lệ";
  const cleanedTimestamp = timestamp.replace(/\+.*$/, "Z");
  const date = new Date(cleanedTimestamp);
  if (isNaN(date)) return "Ngày không hợp lệ";
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

.rounded-3xl {
  border-radius: 1.75rem;
}

.rounded-3xl::before {
  content: "";
  position: absolute;
  bottom: 0;
  width: 0;
  height: 0;
  border: 0.875rem solid transparent;
}

.bg-gradient-to-r.from-green-500::before {
  right: -0.875rem;
  border-bottom-color: #10b981;
  border-right-color: transparent;
}

.bg-white\/95::before {
  left: -0.875rem;
  border-bottom-color: rgba(255, 255, 255, 0.95);
  border-left-color: transparent;
}

input:focus {
  box-shadow: 0 0 0 4px rgba(16, 185, 129, 0.1);
}

.max-w-2xl {
  transition: transform 0.3s ease-out, opacity 0.3s ease-out;
  opacity: 0.97;
}

.max-w-2xl:hover {
  transform: translateY(-4px);
  opacity: 1;
}

@keyframes fadeIn {
  from {
    opacity: 0;
    transform: translateY(12px);
  }
  to {
    opacity: 0.97;
    transform: translateY(0);
  }
}

.animate-fadeIn {
  animation: fadeIn 0.4s ease-out;
}
</style>
