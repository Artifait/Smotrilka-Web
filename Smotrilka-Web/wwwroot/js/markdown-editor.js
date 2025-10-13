class MarkdownEditor {
    constructor(textareaId, previewId) {
        this.textarea = document.getElementById(textareaId);
        this.preview = document.getElementById(previewId);

        if (this.textarea && this.preview) {
            this.init();
        }
    }

    init() {
        this.createToolbar();
        this.textarea.addEventListener('input', () => {
            this.updatePreview();
        });
        this.updatePreview();
    }

    createToolbar() {
        const toolbar = document.createElement('div');
        toolbar.className = 'markdown-toolbar';
        toolbar.innerHTML = `
            <div class="toolbar-group">
                <button type="button" data-action="bold" title="Жирный текст"><strong>B</strong></button>
                <button type="button" data-action="italic" title="Курсив"><em>I</em></button>
                <button type="button" data-action="header" title="Заголовок">H</button>
            </div>
            <div class="toolbar-group">
                <button type="button" data-action="link" title="Ссылка">🔗</button>
                <button type="button" data-action="code" title="Код">{"}"}</button>
            </div>
            <div class="toolbar-group">
                <button type="button" data-action="list" title="Список">•</button>
                <button type="button" data-action="quote" title="Цитата">"</button>
            </div>
        `;

        this.textarea.parentNode.insertBefore(toolbar, this.textarea);
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
        let cursorOffset = 0;

        switch (action) {
            case 'bold':
                newText = `**${selectedText}**`;
                cursorOffset = 2;
                break;
            case 'italic':
                newText = `*${selectedText}*`;
                cursorOffset = 1;
                break;
            case 'header':
                newText = `## ${selectedText}`;
                cursorOffset = 3;
                break;
            case 'link':
                newText = `[${selectedText}](https://)`;
                cursorOffset = 3;
                break;
            case 'code':
                if (selectedText.includes('\n')) {
                    newText = `\`\`\`\n${selectedText}\n\`\`\``;
                    cursorOffset = 4;
                } else {
                    newText = `\`${selectedText}\``;
                    cursorOffset = 1;
                }
                break;
            case 'list':
                newText = selectedText.split('\n').map(line => `- ${line}`).join('\n');
                cursorOffset = 2;
                break;
            case 'quote':
                newText = selectedText.split('\n').map(line => `> ${line}`).join('\n');
                cursorOffset = 2;
                break;
        }

        this.textarea.value = this.textarea.value.substring(0, start) + newText + this.textarea.value.substring(end);
        this.textarea.focus();

        if (selectedText) {
            this.textarea.selectionStart = start + newText.length;
            this.textarea.selectionEnd = start + newText.length;
        } else {
            this.textarea.selectionStart = start + cursorOffset;
            this.textarea.selectionEnd = start + cursorOffset;
        }

        this.updatePreview();
    }

    updatePreview() {
        const markdown = this.textarea.value;
        const html = this.parseMarkdown(markdown);
        this.preview.innerHTML = html || '<span class="preview-placeholder">Предпросмотр появится здесь...</span>';
    }

    parseMarkdown(markdown) {
        if (!markdown.trim()) return '';

        let html = markdown;

        // Заголовки
        html = html.replace(/^### (.*$)/gm, '<h3>$1</h3>');
        html = html.replace(/^## (.*$)/gm, '<h2>$1</h2>');
        html = html.replace(/^# (.*$)/gm, '<h1>$1</h1>');

        // Жирный текст
        html = html.replace(/\*\*(.*?)\*\*/g, '<strong>$1</strong>');

        // Курсив
        html = html.replace(/\*(.*?)\*/g, '<em>$1</em>');

        // Ссылки
        html = html.replace(/\[([^\]]+)\]\(([^)]+)\)/g, '<a href="$2" target="_blank" rel="noopener">$1</a>');

        // Блоки кода
        html = html.replace(/```([^`]+)```/g, '<pre><code>$1</code></pre>');

        // Строчный код
        html = html.replace(/`([^`]+)`/g, '<code>$1</code>');

        // Списки
        html = html.replace(/^- (.*$)/gm, '<li>$1</li>');
        html = html.replace(/(<li>.*<\/li>)/s, '<ul>$1</ul>');

        // Цитаты
        html = html.replace(/^> (.*$)/gm, '<blockquote>$1</blockquote>');

        // Абзацы (обработка переносов строк)
        html = html.split('\n\n').map(paragraph => {
            if (!paragraph.trim()) return '';
            if (!paragraph.startsWith('<') || paragraph.startsWith('<li>') || paragraph.startsWith('<blockquote>') ||
                paragraph.startsWith('<h1>') || paragraph.startsWith('<h2>') || paragraph.startsWith('<h3>') ||
                paragraph.startsWith('<pre>') || paragraph.startsWith('<ul>')) {
                return paragraph;
            }
            return `<p>${paragraph}</p>`;
        }).join('');

        // Одиночные переносы строк
        html = html.replace(/\n(?!\n)/g, '<br>');

        return html;
    }
}

// Инициализация редактора
document.addEventListener('DOMContentLoaded', () => {
    const descriptionTextarea = document.getElementById('description');
    const descriptionPreview = document.getElementById('descriptionPreview');

    if (descriptionTextarea && descriptionPreview) {
        new MarkdownEditor('description', 'descriptionPreview');
    }
});