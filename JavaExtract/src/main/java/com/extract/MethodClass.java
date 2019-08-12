package com.extract;

import com.fasterxml.jackson.annotation.JsonProperty;
import com.github.javaparser.ast.Node;
import com.github.javaparser.ast.body.MethodDeclaration;

public class MethodClass {

    @JsonProperty
    private int startLine;

    @JsonProperty
    private int endLine;

    private NodeSerialize tree;

    private String processedContent;

    @JsonProperty
    private String baseMethod;

    @JsonProperty
    private String methodName;

    public MethodClass(int startLine, int toLine, Node baseMethod) {
        this.startLine = startLine;
        this.endLine = toLine;
        this.baseMethod = baseMethod.toString();
        this.methodName = ((MethodDeclaration) baseMethod).getNameAsString();
    }

    public NodeSerialize getTree() {
        return tree;
    }

    public void setTree(NodeSerialize tree) {
        this.tree = tree;
    }

    public String getProcessedContent() {
        return processedContent;
    }

    public void setProcessedContent(String processedContent) {
        this.processedContent = processedContent;
    }
}
