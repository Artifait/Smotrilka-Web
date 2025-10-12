class MarkdownEditor {
    constructor(textareaId, previewId) {
        this.textarea = document.getElementById(textareaId);
        this.preview = document.getElementById(previewId);

        if (this.textarea && this.preview) {
            this.init();
        }
    }

    init() {
        // Создаем тулбар для редактора
        this.createToolbar();

        // Обработка ввода
        this.textarea.addEventListener('input', () => {
            this.updatePreview();
        });

        // Инициализация предпросмотра
        this.updatePreview();
    }

    createToolbar() {
        const toolbar = document.createElement('div');
        toolbar.className = 'markdown-toolbar';
        toolbar.innerHTML = `
            <button type="button" data-action="bold" title="Жирный текст">B</button>
            <button type="button" data-action="italic" title="Курсив">I</button>
            <button type="button" data-action="header" title="Заголовок">H</button>
            <button type="button" data-action="link" title="Ссылка">🔗</button>
            <button type="button" data-action="code" title="Код">{"}"}</button>
            <button type="button" data-action="list" title="Список">•</button>
            <button type="button" data-action="quote" title="Цитата">"</button>
        `;

        this.textarea.parentNode.insertBefore(toolbar, this.textarea);

        // Обработчики кнопок
        toolbar.addEventListener('click', (e) => {
            if (e.target.tagName === 'BUTTON') {
                const action = e.target.dataset.action;
                this.handleAction(action);
            }
        });
    }

    handleAction(action) {
        const start = this.textarea.selectionStart;
        const end = this.textarea.selectionEnd;
        const selectedText = this.textarea.value.substring(start, end);
        let newText = '';

        switch (action) {
            case 'bold':
                newText = `**${selectedText}**`;
                break;
            case 'italic':
                newText = `*${selectedText}*`;
                break;
            case 'header':
                newText = `## ${selectedText}`;
                break;
            case 'link':
                newText = `[${selectedText}](https://)`;
                break;
            case 'code':
                newText = selectedText.includes('\n') ? `\`\`\`\n${selectedText}\n\`\`\`` : `\`${selectedText}\``;
                break;
            case 'list':
                newText = selectedText.split('\n').map(line => `- ${line}`).join('\n');
                break;
            case 'quote':
                newText = selectedText.split('\n').map(line => `> ${line}`).join('\n');
                break;
        }

        this.textarea.value = this.textarea.value.substring(0, start) + newText + this.textarea.value.substring(end);
        this.textarea.focus();
        this.textarea.selectionStart = start + newText.length;
        this.textarea.selectionEnd = start + newText.length;

        this.updatePreview();
    }

    updatePreview() {
        const markdown = this.textarea.value;
        const html = this.parseMarkdown(markdown);
        this.preview.innerHTML = html;
    }

    parseMarkdown(markdown) {
        // Базовая обработка Markdown
        return markdown
            .replace(/&/g, '&amp;')
            .replace(/</g, '&lt;')
            .replace(/>/g, '&gt;')
            .replace(/^(#{1,6})\s*(.*?)\s*#*\s*$/gm, (match, hashes, text) => {
                const level = hashes.length;
                return `<h${level}>${text}</h${level}>`;
            })
            .replace(/\*\*(.*?)\*\*/g, '<strong>$1</strong>')
            .replace(/\*(.*?)\*/g, '<em>$1</em>')
            .replace(/`([^`]+)`/g, '<code>$1</code>')
            .replace(/```([^`]+)```/g, '<pre><code>$1</code></pre>')
            .replace(/\[([^\]]+)\]\(([^)]+)\)/g, '<a href="$2" target="_blank">$1</a>')
            .replace(/^- (.*$)/gm, '<li>$1</li>')
            .replace(/(<li>.*<\/li>)/s, '<ul>$1</ul>')
            .replace(/> (.*$)/gm, '<blockquote>$1</blockquote>')
            .replace(/\n/g, '<br>');
    }
}

// Инициализация редактора при загрузке страницы
document.addEventListener('DOMContentLoaded', () => {
    const descriptionTextarea = document.getElementById('description');
    const descriptionPreview = document.getElementById('descriptionPreview');

    if (descriptionTextarea && descriptionPreview) {
        new MarkdownEditor('description', 'descriptionPreview');
    }
});